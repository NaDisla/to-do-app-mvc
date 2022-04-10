﻿using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using ToDoApp_MVC.Models;
using ToDoApp_MVC.Models.ViewModels;

namespace ToDoApp_MVC.Controllers
{
    public class HomeController : Controller
    {
        string query = "";
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
            
        }

        #region Get All Todos
        public TodoViewModel GetAllTodos()
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
        #endregion

        #region Insert
        public IActionResult Insert(Todo todo)
        {
            if (!string.IsNullOrEmpty(todo.Name))
            {
                query = $"INSERT INTO Todo (Name, AddedDate) VALUES ('{todo.Name}', '{DateTime.Now}')";
                ManageData(query);
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index));
        }
        #endregion

        #region Clear Todos
        [HttpPost]
        public JsonResult Clear()
        {
            query = $"DELETE FROM Todo";
            ManageData(query);
            return Json(new { });
        }
        #endregion

        #region Populate Form
        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            var todo = GetTaskById(id);
            return Json(todo);
        }
        #endregion

        #region Get Task By ID
        private Todo GetTaskById(int id)
        {
            Todo getTodo = new Todo();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=db_todo.db"))
            {
                using (var tableCmd = conn.CreateCommand())
                {
                    conn.Open();
                    tableCmd.CommandText = $"SELECT * FROM Todo WHERE ID = '{id}'";

                    using (var table = tableCmd.ExecuteReader())
                    {
                        if (table.HasRows)
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
        #endregion

        #region Update
        public IActionResult Update(Todo todo)
        {
            query = $"UPDATE Todo SET Name = '{todo.Name}', AddedDate = '{DateTime.Now}' WHERE ID = {todo.ID}";
            ManageData(query);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            query = $"DELETE FROM Todo WHERE ID = '{id}'";
            ManageData(query);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ManageData
        public void ManageData(string query)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=db_todo.db"))
            {
                using (var tableCmd = conn.CreateCommand())
                {
                    conn.Open();
                    tableCmd.CommandText = query;
                    tableCmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

    }
}