﻿using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services
{
    public class ResultService : IResultService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDataBaseContext;
        private readonly ICommonDbService _commonDbService;

        public ResultService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService)
        {
            _dataContextFactory = dataContextFactory;
            _commonDbService = commonDbService;
            _commonDataBaseContext = _commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();

        }

        public void ShortlistNomination(int nominationId)
        {
            var shortlistedNomination = new Shortlist()
            {
                IsWinner = false,
                NominationId = nominationId,
            };
            _encourageDatabaseContext.Add<Shortlist>(shortlistedNomination);
        }

        public void SelectWinner(int nominationId)
        {
            throw new NotImplementedException();
        }


        public int IsShortlistedOrWinner(int nominationId)
        {
            var shortlisted = _encourageDatabaseContext.Query<Shortlist>().Where(model => model.NominationId == nominationId).SingleOrDefault();

            // if returns 1 then it is winner,and if returns 2 then it is shortlisted already but not winner

            if (shortlisted != null)
            {
                if (shortlisted.IsWinner==true)
                {
                    return 1;
                }
                return 2;
            }
            else
                return 0;
        }
    }
}
