namespace SandProject
{
    public class Book
    {
        public string Name { get; set; }

        public int Something { get; set; }

        public Author Author { get; set; }

        public override string ToString()
        {
            return $"Name: {this.Name}\r\nSomething: {this.Something}\r\n{this.Author.ToString()}";
        }
    }
}
