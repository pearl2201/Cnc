using System;
using System.ComponentModel.DataAnnotations;

namespace Cnc.Client.Entities
{
    public class ClientInfo
    {
        public string Username { get; set; }

        public string OperationSystem { get; set; }

        public string ComputerName { get; set; }

        public string MacAddress { get; set; }
    }
}