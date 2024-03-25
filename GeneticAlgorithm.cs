using System;
using System.Collections.Generic;

namespace GeneticAlgorithmm
{
    public class GeneticAlgorithm
    {
        private int populationSize;
        private double crossoverRate;
        private double mutationRate;
        private int eliteSize;
        private int generationCount;
        private Random random;
        private Chromosome bestSolution;
        private List<double> bestFitnessHistory; // En iyi fitness değerlerinin sakladığım liste

        public GeneticAlgorithm(int populationSize, double crossoverRate, double mutationRate, int eliteSize, int generationCount)
        {
            this.populationSize = populationSize;
            this.crossoverRate = crossoverRate;
            this.mutationRate = mutationRate;
            this.eliteSize = eliteSize;
            this.generationCount = generationCount;
            this.random = new Random();
            this.bestFitnessHistory = new List<double>(); //list
        }

        public Chromosome GetBestSolution
        {
            get { return bestSolution; }
        }

        public List<double> GetBestFitnessHistory()
        {
            return bestFitnessHistory;
        }

        public void Solve()
        {
            List<Chromosome> population = InitializePopulation();
            for (int i = 0; i < generationCount; i++)
            {
                foreach (var chromosome in population)
                {
                    chromosome.Fitness = CalculateFitness(chromosome.X, chromosome.Y);
                }

                population.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));

                List<Chromosome> elites = population.GetRange(0, eliteSize);

                List<Chromosome> newPopulation = new List<Chromosome>();
                while (newPopulation.Count < populationSize)
                {
                    Chromosome parent1 = SelectParent(population);
                    Chromosome parent2 = SelectParent(population);
                    Chromosome child = Crossover(parent1, parent2);
                    Mutate(child);
                    newPopulation.Add(child);
                }

                newPopulation.AddRange(elites);
                population = newPopulation;

                
                FindBestSolution(population);
              
                bestFitnessHistory.Add(bestSolution.Fitness);
            }

           
            Console.WriteLine("En iyi çözüm:");
            Console.WriteLine($"X = {bestSolution.X}, Y = {bestSolution.Y}");
            Console.WriteLine($"Amaç Fonksiyon Değeri: {bestSolution.Fitness}");
        }

        private List<Chromosome> InitializePopulation()
        {
            List<Chromosome> population = new List<Chromosome>();
            for (int i = 0; i < populationSize; i++)
            {
                double x = random.NextDouble() * 20 - 10;
                double y = random.NextDouble() * 20 - 10;
                population.Add(new Chromosome(x, y));
            }
            return population;
        }

        private double CalculateFitness(double x, double y)
        {
            
            x = Math.Max(-10, Math.Min(10, x));
            y = Math.Max(-10, Math.Min(10, y));

            
            return Math.Sin(3 * Math.PI * x) * Math.Sin(3 * Math.PI * x) +
                   (x - 1) * (x - 1) * (1 + Math.Sin(3 * Math.PI * y) * Math.Sin(3 * Math.PI * y)) +
                   (y - 1) * (y - 1) * (1 + Math.Sin(2 * Math.PI * y) * Math.Sin(2 * Math.PI * y));


        }


        private Chromosome SelectParent(List<Chromosome> population)
        {
            // Turnuva seçimi için rastgele seçilecek birey sayısı
            int tournamentSize = 2;

            // Turnuva seçimi yapılacak bireyleri rastgele seç
            List<Chromosome> tournamentParticipants = new List<Chromosome>();
            for (int i = 0; i < tournamentSize; i++)
            {
                int index = random.Next(population.Count);
                tournamentParticipants.Add(population[index]);
            }

            // Her iki kromozomun da uygun aralıkta olup olmadığını kontrol et
            bool parent1InBounds = IsInBounds(tournamentParticipants[0]);
            bool parent2InBounds = IsInBounds(tournamentParticipants[1]);

            // Eğer her iki kromozom da uygun aralıkta değilse, penalty fonksiyonu sonucu daha küçük olan bireyi al
            if (!parent1InBounds && !parent2InBounds)
            {
                return tournamentParticipants.OrderBy(chromosome => chromosome.Fitness).First();
            }
            // Eğer yalnızca bir kromozom uygun aralıkta ise, penalty fonksiyonu sonucu sıfırsa, bu kromozomu al
            else if (parent1InBounds ^ parent2InBounds)
            {
                return parent1InBounds ? tournamentParticipants[0] : tournamentParticipants[1];
            }
            // Eğer her iki kromozom da uygun aralıkta ise, daha uygun olanı seç
            else
            {
                return tournamentParticipants.OrderBy(chromosome => chromosome.Fitness).First();
            }
        }

        // Verilen kromozomun uygun aralıkta olup olmadığını kontrol eden kısım
        private bool IsInBounds(Chromosome chromosome)
        {
            // Kromozomun X ve Y değerlerini al
            double x = chromosome.X;
            double y = chromosome.Y;

            // Uygun aralıkları kontrol et 
            return x >= -10 && x <= 10 && y >= -10 && y <= 10;
        }

        private Chromosome Crossover(Chromosome parent1, Chromosome parent2)
        {
            double x = random.NextDouble() < crossoverRate ? parent1.X : parent2.X;
            double y = random.NextDouble() < crossoverRate ? parent1.Y : parent2.Y;
            return new Chromosome(x, y);
        }

        private void Mutate(Chromosome chromosome)
        {
            if (random.NextDouble() < mutationRate)
            {
                chromosome.X = random.NextDouble() * 20 - 10;
            }
            if (random.NextDouble() < mutationRate)
            {
                chromosome.Y = random.NextDouble() * 20 - 10;
            }
        }

        private void FindBestSolution(List<Chromosome> population)
        {
            bestSolution = population[0];
        }
    }

    public class Chromosome
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Fitness { get; set; }

        public Chromosome(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
