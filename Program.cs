using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

bool runProgram = true;

using (var db = new ToDoContext())
{
    db.Database.EnsureCreated();

    Console.WriteLine("Type in your first name");
    string firstName = Console.ReadLine();

    Console.WriteLine("Type in your last name");
    string lastName = Console.ReadLine();

    var user = db.Users.Include(u => u.ToDoItems).FirstOrDefault(u => u.FirstName == firstName && u.LastName == lastName);
    if (user == null)
    {
        user = new User { FirstName = firstName, LastName = lastName };
        db.Users.Add(user);
        db.SaveChanges();
    }

    while (runProgram)
    {
        foreach (var item in user.ToDoItems)
        {
            Console.WriteLine(item.Task);
        }

        Console.WriteLine("Add an item to the to do list (type 'stop' to quit)");
        var task = Console.ReadLine();
        if (task == "stop")
        {
            runProgram = false;
        }
        else if (!string.IsNullOrEmpty(task))
        {
            var toDoItem = new ToDoItem { Task = task, UserId = user.Id };
            db.ToDoItems.Add(toDoItem);
            db.SaveChanges();
        }
    }
}

public class ToDoItem
{
    public int Id { get; set; }
    public string Task { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<ToDoItem> ToDoItems { get; set; }

    public User()
    {
        ToDoItems = new List<ToDoItem>();
    }
}

public class ToDoContext : DbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=todo.db");
    }
}

//Pakker
//dotnet add package Microsoft.EntityFrameworkCore.Sqlite
//dotnet add package Microsoft.EntityFrameworkCore.Design

//using Microsoft.EntityFrameworkCore;


//dotnet ef migrations add AddFirstNameAndLastName
//dotnet ef database update