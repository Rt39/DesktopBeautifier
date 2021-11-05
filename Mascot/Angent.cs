namespace Mascot
{
    public class Angent
    {
        public string Name { set; get; }
        public int Status;
        private string ResDir;
        public Angent(string Name)
        {
            this.Name = Name;
            Status = 0;
            ResDir = $"../../Resources/{Name}/frame";
        }
        public string GetDir()
        {
            return this.ResDir;
        }
    }
}
