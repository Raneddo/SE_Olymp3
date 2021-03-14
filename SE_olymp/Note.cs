namespace SE_olymp
{
    public class Note
    {
        public Note(int? id, string title, string content)
        {
            Id = id;
            Title = title;
            Content = content;
        }

        public int? Id { get; }
        public string Title { get; }
        public string Content { get; }
    }
}