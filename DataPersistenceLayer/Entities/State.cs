﻿using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class State
    {
        [Key]
        public int IdState { get; set; }

        [MaxLength(25)]
        public string NameState { get; set; }
    }
}
