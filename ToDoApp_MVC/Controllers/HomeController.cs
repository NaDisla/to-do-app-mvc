using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.Diagnostics;
using ToDoApp_MVC.Models;
using ToDoApp_MVC.Models.ViewModels;

namespace ToDoApp_MVC.Controllers
{
    public class HomeController : Controller
    {
        
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
        }
        internal TodoViewModel GetAllTodos()
        {
            List<Todo> todoList = new List<Todo>();

            using (SQLiteConnection conn = new SQLiteConnection($"Data Source = db_todo.db"))
            {
                using (var tableCmd = conn.CreateCommand())
                {
                    conn.Open();
                    tableCmd.CommandText = $"SELECT * FROM Todo";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                todoList.Add(new Todo
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    AddedDate = reader.GetString(2)
                                });
                            }
                        }
                        else
                        {
                            return new TodoViewModel
                            {
                                TodoList = todoList
                            };
                        }
                    }
                }
                return new TodoViewModel
                {
                    TodoList = todoList
                };
            }
        }
        public IActionResult Insert(Todo todo)
        {
            //if(ModelState.IsValid)
            //{
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=db_todo.db"))
                {
                    using (var tableCmd = conn.CreateCommand())
                    {
                        conn.Open();
                        tableCmd.CommandText = $"INSERT INTO Todo (Name, AddedDate) VALUES ('{todo.Name}', '{DateTime.Now}')";
                        try
                        {
                            tableCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            //return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public JsonResult Clear()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=db_todo.db"))
            {
                using (var tableCmd = conn.CreateCommand())
                {
                    conn.Open();
                    tableCmd.CommandText = $"DELETE FROM Todo";
                    tableCmd.ExecuteNonQuery();
                }
            }
            return Json(new { });
        }
        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            var todo = GetTaskById(id);
            return Json(todo);
        }

        private Todo GetTaskById(int id)
        {
            Todo getTodo = new Todo();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=db_todo.db"))
            {
                using (var tableCmd = conn.CreateCommand())
                {
                    conn.Open();
                    tableCmd.CommandText = $"SELECT * FROM Todo WHERE ID = '{id}'";

                    using(var table = tableCmd.ExecuteReader())
                    {
                        if(table.HasRows)
                        {
                            table.Read();
                            getTodo.ID = table.GetInt32(0);
                            getTodo.Name = table.GetString(1);
                            getTodo.AddedDate = table.GetString(2);
                        }
                        else
                        {
                            return getTodo;
                        }
                    }
                }
            }
            return getTodo;
        }
        public IActionResult Update(Todo todo)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=db_todo.db"))
            {
                using (var tableCmd = conn.CreateCommand())
                {
                    conn.Open();
                    tableCmd.CommandText = $"UPDATE Todo SET Name = '{todo.Name}', AddedDate = '{DateTime.Now}' WHERE ID = {todo.ID}";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=db_todo.db"))
            {
                using (var tableCmd = conn.CreateCommand())
                {
                    conn.Open();
                    tableCmd.CommandText = $"DELETE FROM Todo WHERE ID = '{id}'";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}