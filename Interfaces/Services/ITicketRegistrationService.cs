﻿using DataLayer.Model;
using EventSeller.DataLayer.EntitiesDto.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces.Services
{
    public interface ITicketRegistrationService
    {
        // all seats at hall with constraints( for circus with rows start and end )
        Task<IEnumerable<Ticket>> AddTicketsForPlaceHallForAllSeatsAsync(AddTicketsForHallToFillDTO addTicketsForHallToFillDTO);
        //directly for hall and count
        Task<IEnumerable<Ticket>> AddTicketsForPlaceHallByCountAsync(AddTicketsForHallByCountDTO addTicketsForHallByCountDTO);

    }
}
