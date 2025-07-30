using TaskManagement.Api.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Api.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskModel> GetAllTasks();
        TaskModel GetTaskById(int id);
        void CreateTask(TaskModel task);
        void UpdateTask(TaskModel task);
        void DeleteTask(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly TaskContext _context;

        public TaskService(TaskContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }

        public TaskModel GetTaskById(int id)
        {
            return _context.Tasks.Find(id);
        }

        public void CreateTask(TaskModel task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(TaskModel task)
        {
            _context.Entry(task).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            } catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    throw new Exception("Task Not Found");
                } else
                {
                    throw;
                }
            }
        }

        public void DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                throw new Exception("Task Not Found");
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
