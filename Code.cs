public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public class StudentRepository
{
    private readonly string _connectionString;

    public StudentRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Student> GetAll()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Students", connection);
            var reader = command.ExecuteReader();
            var students = new List<Student>();
            while (reader.Read())
            {
                var student = new Student
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Age = (int)reader["Age"]
                };
                students.Add(student);
            }
            return students;
        }
    }

    public Student GetById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Students WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var student = new Student
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Age = (int)reader["Age"]
                };
                return student;
            }
            return null;
        }
    }

    public void Add(Student student)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("INSERT INTO Students (Name, Age) VALUES (@name, @age)", connection);
            command.Parameters.AddWithValue("@name", student.Name);
            command.Parameters.AddWithValue("@age", student.Age);
            command.ExecuteNonQuery();
        }
    }

    public void Update(Student student)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("UPDATE Students SET Name = @name, Age = @age WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@name", student.Name);
            command.Parameters.AddWithValue("@age", student.Age);
            command.Parameters.AddWithValue("@id", student.Id);
            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("DELETE FROM Students WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}


var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=School;Integrated Security=True";
var studentRepository = new StudentRepository(connectionString);

var students = studentRepository.GetAll();


var student = studentRepository.GetById(1);

// Agregar un nuevo estudiante
var newStudent = new Student { Name = "Juan", Age = 20 };
studentRepository.Add(newStudent);

// Actualizar un estudiante existente
var existingStudent = studentRepository.GetById(1);
existingStudent.Name = "Juan Pablo";
existingStudent.Age = 21;
studentRepository.Update(existingStudent);

// Eliminar un estudiante por id
studentRepository.Delete(1);

