using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MDK;
using System.IO;

namespace micro6
{
    public class GammaEnergy : MDK.GraphicPrimitive
    {
        #region Программное

        public string sName = "GammaEnergy";
        public override string ExpirementName = "Исследование энергии гамма-квантов при расспаде нейтрального пиона";
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

        public override void Draw(System.Windows.Forms.PaintEventArgs e)
        {
            Pairs.Clear();
            GammaPair.bottom = this.Top + this.Size.Height;
            GammaPair.middle = this.Size.Width / 2;
            
            Random rnd = new Random(); 
            

            for (int ThrowCounter = 0; ThrowCounter < throws; ThrowCounter++)
            {

                double energy = (double)rnd.Next((int)lEnergy, (int)hEnergy);

                GammaPair p = new GammaPair(energy);

                bool flg = false;
                //if (Pairs.Contains(p)) Pairs[Pairs.IndexOf(p)].ParticlesCount++;
                foreach(GammaPair g in Pairs)
                {
                    if (g == p)
                    {
                        g.ParticlesCount += 20;
                        flg = true;
                    }
                }

                if (!flg)  Pairs.Add(p);
                
            }
            

            foreach (GammaPair l in Pairs)
            {
                DrawGate(l.PointsToDraw(), e.Graphics);
            }
            DrawEnvelope(e.Graphics);

        }

        
        void DrawEnvelope(Graphics g)
        {
            Dictionary<int, int> Envelope = new Dictionary<int,int>(); // x:y

            Pairs.Sort(new Comparison<GammaPair>(GateCompare));
            int bottom = GammaPair.bottom;
            int middle = GammaPair.middle;
            
            for (int i = 0; i < Pairs.Count; i++)
            {
                try
                {
                    Envelope.Add((int)Pairs[i].Minor.Energy,
                                                   bottom - (Pairs[i].ParticlesCount) / 20);

                    Envelope.Add((int)Pairs[i].Major.Energy,
                                  bottom - (Pairs[i].ParticlesCount) / 20);
                }
                catch (Exception e) { }
                for (int j = i; j < Pairs.Count; j++)
                {
                    if(Pairs[j].IsWiderThan(Pairs[i]))
                    {
                        try
                        {
                            if (!Envelope.ContainsKey((int)Pairs[i].Minor.Energy) && !Envelope.ContainsKey((int)Pairs[i].Major.Energy))
                            {
                                Envelope.Add((int)Pairs[i].Minor.Energy,
                                               bottom - (Pairs[i].ParticlesCount + Pairs[j].ParticlesCount) / 20);

                                Envelope.Add((int)Pairs[i].Major.Energy,
                                              bottom - (Pairs[i].ParticlesCount + Pairs[j].ParticlesCount) / 20);

                            }
                            else
                            {
                                Envelope[(int)Pairs[i].Minor.Energy] -= Pairs[j].ParticlesCount / 20;
                                Envelope[(int)Pairs[i].Major.Energy] -= Pairs[j].ParticlesCount / 20;
                            }
                        }
                        catch (Exception e)
                        { }
                    }
                }
            }
            Dictionary<int, int>.KeyCollection keys = Envelope.Keys;
            foreach (int a in keys)
            {
                g.DrawRectangle(Pens.Red, middle + a, Envelope[a]/5 + 300, 1, 1);
            }


        }

        int PointCompare(Point a, Point b)
        {
            if (a.X > b.X) return 1;
            else if (a.X < b.X) return -1;
            else return 0;
        }

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
