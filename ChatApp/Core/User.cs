namespace ChatApp.Core
{
    /// <summary>
    /// Represents a user of the chat system, identified by their name.
    /// </summary>
    public class User
    {
        public string Name { get; private set; }

        public User(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
