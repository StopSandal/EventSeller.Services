﻿using AutoMapper;
using EventSeller.DataLayer.Entities;
using DataLayer.Model.EF;
using EventSeller.DataLayer.EntitiesDto.Statistics;
using EventSeller.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DataLayer.Model;

namespace EventSeller.Services.Repository
{
    internal class AnalyticsRepositoryAsync : IAnalyticsRepositoryAsync
    {
        private readonly SellerContext _context;

        public AnalyticsRepositoryAsync(SellerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventPopularityStatistic>> GetEventsWithMaxPopularityAsync(Expression<Func<Event, bool>>? eventsFilter = null, Expression<Func<EventPopularityStatistic, decimal>>? orderBy = null, int maxCount = 0)
        {
            IQueryable<Event> events = _context.Set<Event>();

            if (eventsFilter != null)
            {
                events.Where(eventsFilter);
            }

            var query = events
                .Select(eventEntity => new
                {
                    Event = eventEntity,
                    TotalSold = eventEntity.EventSessions
                        .SelectMany(es => es.Tickets)
                        .Count(),
                    SoldCount = eventEntity.EventSessions
                        .SelectMany(es => es.Tickets)
                        .Count(ticket => ticket.isSold),
                    TotalIncome = eventEntity.EventSessions
                        .SelectMany(es => es.Tickets)
                        .Where(ticket => ticket.isSold)
                        .Sum(ticket => ticket.Price)
                })
                .Where(e => e.TotalSold > 0)
                .Select(e => new EventPopularityStatistic
                {
                    EventItem = e.Event,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.SoldCount / e.TotalSold,
                        Popularity = ((decimal)e.SoldCount / e.TotalSold) * e.TotalIncome
                    }
                });

            if(orderBy != null)
            {
                query.OrderByDescending(orderBy);
            }

            if (maxCount > 0)
            {
                query.Take(maxCount);
            }

            var eventsWithPopularity = await query.ToListAsync();

            if (eventsWithPopularity == null)
            {
                throw new InvalidDataException("No events found with sold tickets.");
            }

            return eventsWithPopularity;
        }
        public async Task<IEnumerable<EventTypePopularityStatisticDTO>> GetEventTypesWithPopularityAsync(Expression<Func<EventType, bool>>? eventsFilter = null, Expression<Func<EventTypePopularityStatisticDTO, decimal>>? orderBy = null, int maxCount = 0)
        {
            IQueryable<EventType> eventTypes = _context.Set<EventType>();

            if (eventsFilter != null)
            {
                eventTypes.Where(eventsFilter);
            }
            var query = eventTypes
                .Select(eventTypeEntity => new
                {
                    EventType = eventTypeEntity,
                    TotalSold = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Count(),
                    SoldCount = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Count(ticket => ticket.isSold),
                    TotalIncome = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Where(ticket => ticket.isSold)
                        .Sum(ticket => ticket.Price)
                })
                .Where(e => e.TotalSold > 0)
                .Select(e => new EventTypePopularityStatisticDTO
                {
                    EventTypeItem = e.EventType,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.SoldCount / e.TotalSold,
                        Popularity = ((decimal)e.SoldCount / e.TotalSold) * e.TotalIncome
                    }
                });

            if (orderBy != null)
            {
                query.OrderByDescending(orderBy);
            }

            if (maxCount > 0)
            {
                query.Take(maxCount);
            }

            var eventsWithPopularity = await query.ToListAsync();

            if (eventsWithPopularity == null)
            {
                throw new InvalidDataException("No event types found with sold tickets.");
            }

            return eventsWithPopularity;
        }
        public async Task<EventTypePopularityStatisticDTO> GetEventTypeWithMaxPopularityAsync(Expression<Func<EventType, bool>> eventTypeFilter)
        {
            var events = _context.Set<EventType>()
                            .Where(eventTypeFilter);
            var eventsWithPopularity = await events
                .Select(eventTypeEntity => new
                {
                    EventType = eventTypeEntity,
                    TotalSold = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Count(),
                    SoldCount = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Count(ticket => ticket.isSold),
                    TotalIncome = eventTypeEntity.Event
                        .SelectMany(e => e.EventSessions)
                        .SelectMany(es => es.Tickets)
                        .Where(ticket => ticket.isSold)
                        .Sum(ticket => ticket.Price)
                })
                .Where(e => e.TotalSold > 0)
                .Select(e => new EventTypePopularityStatisticDTO
                {
                    EventTypeItem = e.EventType,
                    PopularityStatistic = new PopularityStatisticDTO
                    {
                        Realization = (decimal)e.SoldCount / e.TotalSold,
                        Popularity = ((decimal)e.SoldCount / e.TotalSold) * e.TotalIncome
                    }
                })
                .FirstOrDefaultAsync();

            if (eventsWithPopularity == null)
            {
                throw new InvalidDataException("No events found with sold tickets.");
            }

            return eventsWithPopularity;
        }
    }
}