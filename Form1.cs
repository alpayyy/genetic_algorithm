using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GeneticAlgorithmm
{
    public partial class Form1 : Form
    {
        private GeneticAlgorithm geneticAlgorithm;
        private List<double> bestFitnessHistory;

        public Form1()
        {
            InitializeComponent();
            bestFitnessHistory = new List<double>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kullanýcý tarafýndan girilen parametreleri alýn
            int populationSize, eliteSize, generationCount;
            double crossoverRate, mutationRate;

            if (!int.TryParse(textBox1.Text, out populationSize) || populationSize <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir nüfus boyutu girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBox2.Text, out crossoverRate) || crossoverRate < 0 || crossoverRate > 1)
            {
                MessageBox.Show("Lütfen geçerli bir çaprazlama oraný girin (0 ile 1 arasýnda).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBox3.Text, out mutationRate) || mutationRate < 0 || mutationRate > 1)
            {
                MessageBox.Show("Lütfen geçerli bir mutasyon oraný girin (0 ile 1 arasýnda).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBox4.Text, out eliteSize) || eliteSize < 0 || eliteSize > populationSize)
            {
                MessageBox.Show("Lütfen geçerli bir elit boyut girin (0 ile nüfus boyutu arasýnda).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBox5.Text, out generationCount) || generationCount <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir nesil sayýsý girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Geçerli parametrelerle genetik algoritmayý baþlat
            geneticAlgorithm = new GeneticAlgorithm(populationSize, crossoverRate, mutationRate, eliteSize, generationCount);

            // Problemi çöz
            geneticAlgorithm.Solve();

            // Sonuçlarý göster
            DisplayResults();

            // Yakýnsama geçmiþini çiz
            PlotFitnessHistory(panel1);
        }


        private void DisplayResults()
        {
            panel1.Controls.Clear();

            Chromosome bestSolution = geneticAlgorithm.GetBestSolution;

            Label labelX = new Label();
            labelX.Text = $"X = {bestSolution.X}";
            labelX.AutoSize = true;
            labelX.Location = new Point(10, 10);
            panel1.Controls.Add(labelX);

            Label labelY = new Label();
            labelY.Text = $"Y = {bestSolution.Y}";
            labelY.AutoSize = true;
            labelY.Location = new Point(10, labelX.Bottom + 10);
            panel1.Controls.Add(labelY);

            Label labelFitness = new Label();
            labelFitness.Text = $"Amaç Fonksiyon Deðeri: {bestSolution.Fitness}";
            labelFitness.AutoSize = true;
            labelFitness.Location = new Point(10, labelY.Bottom + 10);
            panel1.Controls.Add(labelFitness);
        }


        private void PlotFitnessHistory(Panel panel)
        {
            panel.Paint += new PaintEventHandler((sender, e) =>
            {
                Pen pen = new Pen(Color.Blue);

                // Panelin boyutlarýný al
                int panelWidth = panel.Width;
                int panelHeight = panel.Height;

                // X ve Y eksenlerinin baþlangýç ve bitiþ noktalarýný belirle
                int xAxisStart = 50;
                int xAxisEnd = panelWidth - 60;
                int yAxisStart = panelHeight - 60;
                int yAxisEnd = 50;

                // X ve Y eksenlerini çiz
                e.Graphics.DrawLine(pen, xAxisStart, yAxisStart, xAxisEnd, yAxisStart);
                e.Graphics.DrawLine(pen, xAxisStart, yAxisStart, xAxisStart, yAxisEnd);

                // Yakýnsama verilerini çiz
                List<double> bestFitnessHistory = geneticAlgorithm.GetBestFitnessHistory();
                if (bestFitnessHistory.Count > 1)
                {
                    double maxX = bestFitnessHistory.Count - 1;
                    double maxY = bestFitnessHistory[0];

                    for (int i = 0; i < bestFitnessHistory.Count - 1; i++)
                    {
                        float x1 = xAxisStart + (i * (xAxisEnd - xAxisStart) / (float)maxX);
                        float y1 = yAxisStart - (float)(bestFitnessHistory[i] * (yAxisStart - yAxisEnd) / maxY);

                        float x2 = xAxisStart + ((i + 1) * (xAxisEnd - xAxisStart) / (float)maxX);
                        float y2 = yAxisStart - (float)(bestFitnessHistory[i + 1] * (yAxisStart - yAxisEnd) / maxY);

                        e.Graphics.DrawLine(pen, x1, y1, x2, y2);
                    }
                }

                pen.Dispose();
            });

            // Paneli yeniden boyamak için geçersiz kýlma yapýn
            panel.Invalidate();
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
