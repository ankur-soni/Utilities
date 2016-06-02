﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
    public interface IResultService
    {
        void ShortlistNomination(int nominationId);
        void SelectWinner(int nominationId);
        int IsShortlistedOrWinner(int nominationId);
    }
}