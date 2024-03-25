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
            // Kullan�c� taraf�ndan girilen parametreleri al�n
            int populationSize, eliteSize, generationCount;
            double crossoverRate, mutationRate;

            if (!int.TryParse(textBox1.Text, out populationSize) || populationSize <= 0)
            {
                MessageBox.Show("L�tfen ge�erli bir n�fus boyutu girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBox2.Text, out crossoverRate) || crossoverRate < 0 || crossoverRate > 1)
            {
                MessageBox.Show("L�tfen ge�erli bir �aprazlama oran� girin (0 ile 1 aras�nda).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBox3.Text, out mutationRate) || mutationRate < 0 || mutationRate > 1)
            {
                MessageBox.Show("L�tfen ge�erli bir mutasyon oran� girin (0 ile 1 aras�nda).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBox4.Text, out eliteSize) || eliteSize < 0 || eliteSize > populationSize)
            {
                MessageBox.Show("L�tfen ge�erli bir elit boyut girin (0 ile n�fus boyutu aras�nda).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBox5.Text, out generationCount) || generationCount <= 0)
            {
                MessageBox.Show("L�tfen ge�erli bir nesil say�s� girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ge�erli parametrelerle genetik algoritmay� ba�lat
            geneticAlgorithm = new GeneticAlgorithm(populationSize, crossoverRate, mutationRate, eliteSize, generationCount);

            // Problemi ��z
            geneticAlgorithm.Solve();

            // Sonu�lar� g�ster
            DisplayResults();

            // Yak�nsama ge�mi�ini �iz
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
            labelFitness.Text = $"Ama� Fonksiyon De�eri: {bestSolution.Fitness}";
            labelFitness.AutoSize = true;
            labelFitness.Location = new Point(10, labelY.Bottom + 10);
            panel1.Controls.Add(labelFitness);
        }


        private void PlotFitnessHistory(Panel panel)
        {
            panel.Paint += new PaintEventHandler((sender, e) =>
            {
                Pen pen = new Pen(Color.Blue);

                // Panelin boyutlar�n� al
                int panelWidth = panel.Width;
                int panelHeight = panel.Height;

                // X ve Y eksenlerinin ba�lang�� ve biti� noktalar�n� belirle
                int xAxisStart = 50;
                int xAxisEnd = panelWidth - 60;
                int yAxisStart = panelHeight - 60;
                int yAxisEnd = 50;

                // X ve Y eksenlerini �iz
                e.Graphics.DrawLine(pen, xAxisStart, yAxisStart, xAxisEnd, yAxisStart);
                e.Graphics.DrawLine(pen, xAxisStart, yAxisStart, xAxisStart, yAxisEnd);

                // Yak�nsama verilerini �iz
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

            // Paneli yeniden boyamak i�in ge�ersiz k�lma yap�n
            panel.Invalidate();
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
