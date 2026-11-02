namespace UniversityTask5;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Builder — складання ПК:");
        Computer gaming = new ComputerBuilder()
            .WithCpu("AMD Ryzen 7")
            .WithRam(32)
            .WithStorage(1000)
            .WithVideoCard(true)
            .Build();
        Console.WriteLine(gaming);

        Console.WriteLine("\nStrategy — розрахунок знижки:");
        Checkout checkout = new Checkout(new RegularDiscount());
        checkout.ShowTotal(1000m);
        checkout.SetStrategy(new StudentDiscount());
        checkout.ShowTotal(1000m);
        checkout.SetStrategy(new VipDiscount());
        checkout.ShowTotal(1000m);

        Console.WriteLine("\nProxy — ліниве завантаження зображення:");
        IProductImage image = new LazyImageProxy("product.jpg");
        image.Display(); // Завантаження та показ.
        image.Display(); // Повторний показ без завантаження.
    }

    // Builder: налаштовуємо параметри, потім створюємо готовий об'єкт.
    record Computer(string Cpu, int Ram, int Storage, bool HasVideoCard)
    {
        public override string ToString() =>
            $"{Cpu}, {Ram} ГБ RAM, {Storage} ГБ SSD, відеокарта: {(HasVideoCard ? "так" : "ні")}";
    }

    class ComputerBuilder
    {
        private string cpu = "Intel Core i3";
        private int ram = 8;
        private int storage = 256;
        private bool hasVideoCard;

        public ComputerBuilder WithCpu(string value) { cpu = value; return this; }
        public ComputerBuilder WithRam(int value) { ram = value; return this; }
        public ComputerBuilder WithStorage(int value) { storage = value; return this; }
        public ComputerBuilder WithVideoCard(bool value) { hasVideoCard = value; return this; }
        public Computer Build() => new Computer(cpu, ram, storage, hasVideoCard);
    }

    // Strategy: кожен алгоритм знижки реалізує спільний інтерфейс.
    interface IDiscountStrategy
    {
        decimal Apply(decimal price);
    }

    class RegularDiscount : IDiscountStrategy
    {
        public decimal Apply(decimal price) => price;
    }

    class StudentDiscount : IDiscountStrategy
    {
        public decimal Apply(decimal price) => price * 0.9m;
    }

    class VipDiscount : IDiscountStrategy
    {
        public decimal Apply(decimal price) => price * 0.8m;
    }

    class Checkout(IDiscountStrategy strategy)
    {
        public void SetStrategy(IDiscountStrategy value) => strategy = value;
        public void ShowTotal(decimal price) => Console.WriteLine($"{strategy.Apply(price):0.##} грн");
    }

    // Proxy: реальне зображення створюється тільки при першому Display().
    interface IProductImage
    {
        void Display();
    }

    class RealProductImage : IProductImage
    {
        private readonly string fileName;

        public RealProductImage(string fileName)
        {
            this.fileName = fileName;
            Console.WriteLine($"Завантаження: {fileName}");
        }

        public void Display() => Console.WriteLine($"Показ: {fileName}");
    }

    class LazyImageProxy(string fileName) : IProductImage
    {
        private RealProductImage? realImage;

        public void Display()
        {
            realImage ??= new RealProductImage(fileName);
            realImage.Display();
        }
    }
}
