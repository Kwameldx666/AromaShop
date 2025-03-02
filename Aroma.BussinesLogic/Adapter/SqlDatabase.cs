using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;

namespace Aroma.BussinesLogic.Adapter
{
    public class SqlDatabase : IDatabase
    {
        private readonly List<UDbTable> _users = new List<UDbTable>(); // Симуляция базы данных

        public void AddUser(UDbTable User)
        {
            // Предположим, что этот метод добавляет пользователя с дефолтными данными
            _users.Add(User);
            Console.WriteLine("Пользователь добавлен в SQL базу (по умолчанию).");
        }

        public void DeleteUser()
        {
            if (_users.Count > 0)
            {
                _users.RemoveAt(_users.Count - 1); // Удаляем последнего пользователя
                Console.WriteLine("Последний пользователь удалён из SQL базы.");
            }
        }

        public void DeleteUser(int id)
        {
            var UDbTable = _users.Find(u => u.Id == id);
            if (UDbTable != null)
            {
                _users.Remove(UDbTable);
                Console.WriteLine($"Пользователь с ID {id} удалён из SQL базы.");
            }
            else
            {
                throw new Exception($"Пользователь с ID {id} не найден.");
            }
        }

        public void UpdateUser()
        {
            if (_users.Count > 0)
            {
                var UDbTable = _users[_users.Count - 1]; // Последний пользователь
                UDbTable.Username = "UpdatedUDbTable";
                UDbTable.Email = "updated@example.com";
                Console.WriteLine("Последний пользователь обновлён в SQL базе (по умолчанию).");
            }
        }

        // Вспомогательный метод для симуляции (не часть интерфейса)
        public List<UDbTable> GetUDbTables()
        {
            return new List<UDbTable>(_users); // Возвращаем копию списка
        }
    }
}