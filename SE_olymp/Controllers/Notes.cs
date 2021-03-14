using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;

namespace SE_olymp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Notes : ControllerBase
    {
        private readonly ILogger<Notes> _logger;
        private readonly SqliteConnection _dbConnection;

        public Notes(ILogger<Notes> logger)
        {
            _logger = logger;
            _dbConnection = new SqliteConnection("Data Source=db.sqlite;Version=3;");
            _dbConnection.Open();
        }

        [HttpGet]
        public IEnumerable<Note> GetNotes()
        {
            using var cmd = new SqliteCommand("SELECT n.id, n.title, n.content FROM Notes", _dbConnection);
            var reader = cmd.ExecuteReader();

            var idOrdinal = reader.GetOrdinal("id");
            var titleOrdinal = reader.GetOrdinal("title");
            var contentOrdinal = reader.GetOrdinal("content");

            while (reader.Read())
            {
                var id = reader.GetInt32(idOrdinal);
                var title = reader.GetString(titleOrdinal);
                var content = reader.GetString(contentOrdinal);

                yield return new Note(id, title, content);
            }
        }

        [HttpPost]
        public void AddNote([FromBody] string title, [FromBody] string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            
            using var cmd = new SqliteCommand("INSERT INTO Notes (title, content) VALUES ((@title, @content))", _dbConnection);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@content", content);
            cmd.ExecuteNonQuery();
        }
    }
}