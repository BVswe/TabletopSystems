﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.ViewModels
{
    public class AddMonsterViewModel : ObservableObject
    {
        //Header is for dropdown to read viewmodel's name
        public string Header { get; }
        public AddMonsterViewModel()
        {
            Header = "Monster";
        }
    }
}