using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Event;
using DataLayer.Models.HallSector;
using DataLayer.Models.PlaceAddress;
using DataLayer.Models.PlaceHall;
using DataLayer.Models.Ticket;
using DataLayer.Models.TicketSeat;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto;
using EventSeller.DataLayer.EntitiesDto.User;
using EventSeller.DataLayer.ExternalDTO.PaymentSystem;

namespace Services
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ConfigEntitiesMap();
        }
        private void ConfigEntitiesMap()
        {
            MapEvent();
            MapHallSector();
            MapPlaceAddress();
            MapPlaceHall();
            MapTicketSeat();
            MapTicket();
            MapUser();
            MapProcessResponseToPaymentConfirmation();
        }
        private void MapProcessResponseToPaymentConfirmation() 
        {
            CreateMap<ProcessPaymentResponse, PaymentConfirmationDTO>();
        }
        private void MapUser()
        {
            // EditUserDto -> User
            CreateMap<EditUserDto, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
        private void MapEvent()
        {
            // AddEventDto -> Event
            CreateMap<AddEventDto, Event>();

            // EditEventDto -> Event
            CreateMap<EditEventDto, Event>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
        private void MapHallSector()
        {
            // AddHallSectorDto -> HallSector
            CreateMap<AddHallSectorDto, HallSector>();

            // EditHallSectorDto -> HallSector
            CreateMap<EditHallSectorDto, HallSector>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

        }
        private void MapPlaceAddress()
        {
            // AddPlaceAddressDto -> PlaceAddress
            CreateMap<AddPlaceAddressDto, PlaceAddress>();

            // EditPlaceAddressDto -> PlaceAddress
            CreateMap<EditPlaceAddressDto, PlaceAddress>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

        }
        private void MapPlaceHall()
        {
            // AddPlaceHallDto -> PlaceHall
            CreateMap<AddPlaceHallDto, PlaceHall>();

            // EditPlaceHallDto -> PlaceHall
            CreateMap<EditPlaceHallDto, PlaceHall>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

        }
        private void MapTicketSeat()
        {
            // AddTicketSeatDto -> TicketSeat
            CreateMap<AddTicketSeatDto, TicketSeat>();

            // EditTicketSeatDto -> TicketSeat
            CreateMap<EditTicketSeatDto, TicketSeat>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

        }
        private void MapTicket()
        {
            // AddTicketDto -> Ticket
            CreateMap<AddTicketDto, Ticket>();

            // EditTicketDto -> Ticket
            CreateMap<EditTicketDto, Ticket>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

        }
    }
}
