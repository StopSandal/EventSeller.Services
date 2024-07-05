﻿using EventSeller.DataLayer.EntitiesDto.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    public interface IEventPopularityService
    {
        Task<object> GetEventsPopularityByPeriod(DateTime startDateTime, DateTime endDateTime);
        Task<PopularityStatisticDTO> GetEventTypePopularity(long eventTypeId);
        Task<object> GetMostPopularEvent();
        Task<object> GetMostRealizableEvent();
    }
}
