using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker
{
    public class Entity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }  // Primary Key with AutoIncrement
        public string Question { get; set; }  // Property for storing the question
        public string Answer { get; set; }  // Property for storing the answer
    }
}
