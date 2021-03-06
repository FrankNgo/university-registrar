using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace University.Models
{
  public class Student
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private string _rawDate;

    public Student (string firstName, string lastName, string date, int Id = 0)
    {
      _firstName = firstName;
      _lastName = lastName;
      _rawDate = date;
      _id = Id;
    }

    public string GetFirstName() { return _firstName; }

    public string GetLastName() { return _lastName; }

    public string GetDate() { return _rawDate; }

    public int GetId() { return _id; }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

  public void Delete()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();

    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"DELETE FROM students WHERE id = @StudentId; DELETE FROM roster WHERE student_id = @StudentId;";
    MySqlParameter studentIdParameter = new MySqlParameter();
    studentIdParameter.ParameterName = "@StudentId";
    studentIdParameter.Value = this._id;

    cmd.Parameters.Add(studentIdParameter);
    cmd.ExecuteNonQuery();

    conn.Close();
    if(conn != null)
    {
      conn.Dispose();
    }
  }
  public void Save()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();

    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"INSERT INTO `students` (`first_name`,`last_name`,`raw_date`) VALUES (@FirstName, @LastName, @RawDate);";

    MySqlParameter firstName = new MySqlParameter();
    firstName.ParameterName = "@FirstName";
    firstName.Value = this._firstName;

    MySqlParameter lastName = new MySqlParameter();
    lastName.ParameterName = "@LastName";
    lastName.Value = this._lastName;

    MySqlParameter rawDate = new MySqlParameter();
    rawDate.ParameterName = "@RawDate";
    rawDate.Value = this._rawDate;

    cmd.Parameters.Add(firstName);
    cmd.Parameters.Add(lastName);
    cmd.Parameters.Add(rawDate);

    cmd.ExecuteNonQuery();
    _id = (int) cmd.LastInsertedId;

    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
   }

   public static List<Student> GetAll()
   {
     List<Student> allStudents = new List<Student>{};
     MySqlConnection conn = DB.Connection();
     conn.Open();
     MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"SELECT * FROM students;";
     MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
     while(rdr.Read())
     {
       string firstName = rdr.GetString(0);
       string lastName = rdr.GetString(1);
       string rawDate = rdr.GetString(2);
       int id = rdr.GetInt32(3);
       Student newStudent = new Student(firstName,lastName,rawDate,id);
       allStudents.Add(newStudent);
     }

     conn.Close();
     if (conn != null)
     {
       conn.Dispose();
     }
     return allStudents;
   }

   public override bool Equals(System.Object otherStudent)
   {
     if(!(otherStudent is Student))
     {
       return false;
     }
     else
     {
       Student newStudent = (Student) otherStudent;
       bool firstNameEquality = (this.GetFirstName() == newStudent.GetFirstName());
       bool lastNameEquality = (this.GetLastName() == newStudent.GetLastName());
       bool rawDateEquality = (this.GetDate() == newStudent.GetDate());
       bool idEquality = (this.GetId() == newStudent.GetId());
       return (firstNameEquality && lastNameEquality && rawDateEquality && idEquality);
     }
   }

   public static Student Find(int id)
   {
     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"SELECT * from `students` WHERE id = @thisId;";

     MySqlParameter thisId = new MySqlParameter();
     thisId.ParameterName = "@thisId";
     thisId.Value = id;
     cmd.Parameters.Add(thisId);

     var rdr = cmd.ExecuteReader() as MySqlDataReader;
     int studentId = 0;
     string studentFirstName = "";
     string studentLastName = "";
     string studentRawDate = "";

     while (rdr.Read())
     {
       studentId = rdr.GetInt32(3);
       studentFirstName = rdr.GetString(0);
       studentLastName = rdr.GetString(1);
       studentRawDate = rdr.GetString(2);
     }

     Student foundStudent = new Student(studentFirstName, studentLastName, studentRawDate, studentId);

     conn.Close();
     if (conn != null)
     {
       conn.Dispose();
     }

     return foundStudent;
   }


  }
}
