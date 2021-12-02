﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace StreamingMusic.Models
{
    public class MyData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}