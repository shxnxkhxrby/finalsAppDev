using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace QuizMaker
{
    public class Services
    {
        private const string db_name = "db_local";  // Database name
        private readonly SQLiteAsyncConnection _connection;  // SQLite connection

        // Constructor to initialize the database connection and create the Entity table
        public Services()
        {
            // Create a connection to the SQLite database file
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, db_name));

            // Create the table if it does not exist already
            _connection.CreateTableAsync<Entity>().Wait();
        }

        // Save a list of questions and answers to the database
        public async Task SaveQuestionsAndAnswers(List<string> questions, List<string> answers)
        {
            for (int i = 0; i < questions.Count; i++)
            {
                var entity = new Entity
                {
                    Question = questions[i],  // Set the question from the list
                    Answer = answers[i]       // Set the answer from the list
                };

                // Insert the entity (question-answer pair) into the database
                await _connection.InsertAsync(entity);
            }
        }

        // Retrieve all questions and answers from the database
        public async Task<List<Entity>> GetEntities()
        {
            // Query and return all the entities (questions and answers) from the table
            return await _connection.Table<Entity>().ToListAsync();
        }

        // Retrieve a single question-answer entity by its Id
        public async Task<Entity> GetById(int id)
        {
            // Query and return the entity with the given Id
            return await _connection.Table<Entity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        // Insert a single entity (question-answer pair) into the database
        public async Task Insert(Entity entity)
        {
            // Insert the entity into the table
            await _connection.InsertAsync(entity);
        }

        // Update an existing entity in the database
        public async Task Update(Entity entity)
        {
            // Update the entity in the table
            await _connection.UpdateAsync(entity);
        }

        // Delete an entity from the database
        public async Task Delete(Entity entity)
        {
            // Delete the entity from the table
            await _connection.DeleteAsync(entity);
        }
    }
}
