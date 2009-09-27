using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MDK;
using System.IO;
using ZedGraph;
using System.Windows.Forms;

namespace micro6
{
    public class GammaEnergy : MDK.GraphicPrimitive
    {
        #region Программное

        
        

        public string sName = "GammaEnergy";
        public string ExpirementName = "Исследование энергии гамма-квантов при расспаде нейтрального пиона";
        List<GammaPair> Pairs = new List<GammaPair>();

        public GammaEnergy()
            : base(0, 0, new Size())
        {
            
            AddParametersToTemplate();
        }

        public GammaEnergy(int Left, int Top, Size Size)
            : base(Left, Top, Size)
        {
            this.Left = Left;
            this.Top = Top;

            this.Size = Size;
            
            AddParametersToTemplate();
        }

        private void AddParametersToTemplate()
        {
            ParameterListTemplate = new ParameterList();
            ParameterListTemplate.Add("Минимальная энергия π- мезона", "ePi0_Min", 136);
            ParameterListTemplate.Add("Максимальная энергия π- мезона", "ePi0_Max", 200);
            ParameterListTemplate.Add("Количество бросков", "Throws", 300);
        }

        public override void SetParameters(ParameterList pList)
        {
            lEnergy = pList["ePi0_Min"];
            hEnergy = pList["ePi0_Max"];
            throws  = (int)pList["Throws"];
        }

        public override ParameterList GetParameters()
        {
            ParameterList RetList = ParameterListTemplate;
            RetList["ePi0_Min"] = lEnergy;
            RetList["ePi0_Max"] = hEnergy;
            RetList["Throws"]   = throws;
            return RetList;
        }

        #region ZedGrapg
        ZedGraphControl zgc;
        LineItem curve;
        GraphPane myPane;

        private void CreateGraphControl()
        {
            base.ComponentFlag = true;
            zgc = new ZedGraphControl();
            zgc.Top = 0; zgc.Left = 0;
            zgc.Height = this.Height;
            zgc.Width = this.Width;

            myPane = zgc.GraphPane;

            // set the title and axis labels
            myPane.Title.Text = "Распределение энергий гамма-квантов";
            myPane.XAxis.Title.Text = "Энергия частиц";
            myPane.YAxis.Title.Text = "Кол-во частиц";
            
            base.ctrl = zgc;
        }
        #endregion

        public override void Draw(System.Windows.Forms.PaintEventArgs e)
        {
            CreateGraphControl();
            Pairs.Clear();

            Random rnd = new Random();


            for (int ThrowCounter = 0; ThrowCounter < throws; ThrowCounter++)
            {

                double energy = (double)rnd.Next((int)lEnergy, (int)hEnergy);

                GammaPair p = new GammaPair(energy);

                bool flg = false;
                //if (Pairs.Contains(p)) Pairs[Pairs.IndexOf(p)].ParticlesCount++;
                foreach (GammaPair g in Pairs)
                {
                    if (g == p)
                    {
                        g.ParticlesCount += 20;
                        flg = true;
                    }
                }

                if (!flg) Pairs.Add(p);

            }

            myPane.CurveList.Clear();
            foreach (GammaPair l in Pairs)
            { 
                curve = myPane.AddCurve("Ворота", l.PointsToDraw() , Color.Blue, SymbolType.Default);
                curve.Symbol.Fill = new Fill(Color.White);
                curve.Symbol.Size = 2;
            }
            DrawEnvelope();
            

            myPane.Legend.IsVisible = false;
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0F);
            zgc.AxisChange();
            zgc.Refresh();

        }

        /// <summary>
        /// Метод рисования огибающей
        /// </summary>
        void DrawEnvelope()
        {
            Pairs.Sort(new Comparison<GammaPair>(GateCompare)); //сортируем по количеству частиц
            PointPairList Envelope = new PointPairList(); //график огибающей

            for (int i = 0; i < Pairs.Count; i++)
            {
                PointPair p = new PointPair(Pairs[i].Minor.Energy, Pairs[i].ParticlesCount);
                PointPair k = new PointPair(Pairs[i].Major.Energy, Pairs[i].ParticlesCount);

                for (int j = i; j < Pairs.Count; j++)
                {
                    if (Pairs[j].IsWiderThan(Pairs[i]))
                    {
                        p.Y += Pairs[j].ParticlesCount;
                        k.Y += Pairs[j].ParticlesCount;
                    }
                }
                Envelope.Add(p);
                Envelope.Add(k);
            }
            Envelope.Sort();
            LineItem curve = myPane.AddCurve("Огибающая",Envelope, Color.Red, SymbolType.Default);
            
            curve.Symbol.Fill = new Fill(Color.White); 
            curve.Symbol.Size = 2;
            curve.Line.IsSmooth = true; 
            curve.Line.SmoothTension = 0.5F; 

        }



        /// <summary>
        /// Метод для сравнения точек по оси X
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        int PointCompare(Point a, Point b)
        {
            if (a.X > b.X) return 1;
            else if (a.X < b.X) return -1;
            else return 0;
        }

        /// <summary>
        /// Метод сравнения "ворот" по оси y(по количеству частиц)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        int GateCompare(GammaPair a, GammaPair b)
        {
            if (a.ParticlesCount < b.ParticlesCount) return -1;
            else if (a.ParticlesCount < b.ParticlesCount) return 1;
            else return 0;
        }




        private void DrawGate(Point[] points, Graphics e)
        {
            e.DrawLines(Pens.Purple, points);
        }

        #endregion

        #region Научное

        double lEnergy;
        double hEnergy;
        int throws;




        #endregion
    }
}
