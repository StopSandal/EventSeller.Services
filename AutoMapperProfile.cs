using AutoMapper;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto;
using EventSeller.DataLayer.EntitiesDto.Event;
using EventSeller.DataLayer.EntitiesDto.EventSession;
using EventSeller.DataLayer.EntitiesDto.EventType;
using EventSeller.DataLayer.EntitiesDto.HallSector;
using EventSeller.DataLayer.EntitiesDto.PlaceAddress;
using EventSeller.DataLayer.EntitiesDto.PlaceHall;
using EventSeller.DataLayer.EntitiesDto.Ticket;
using EventSeller.DataLayer.EntitiesDto.TicketSeat;
using EventSeller.DataLayer.EntitiesDto.User;
using EventSeller.DataLayer.ExternalDTO.PaymentSystem;

namespace EventSeller.Services
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
            MapEventSession();
            MapHallSector();
            MapPlaceAddress();
            MapPlaceHall();
            MapTicketSeat();
            MapTicket();
            MapUser();
            MapProcessResponseToPaymentConfirmation();
        }
        private void MapEventType()
        {
            // AddEventTypeDTO -> EventType
            CreateMap<AddEventTypeDTO, EventType>();

            // EditEventTypeDTO -> EventType
            CreateMap<EditEventTypeDTO, EventType>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
        private void MapEventSession()
        {
            // AddEventSessionDTO -> EventSession
            CreateMap<AddEventSessionDTO, EventSession>();

            // EditEventSessionDTO -> EventSession
            CreateMap<EditEventSessionDTO, EventSession>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
        private void MapProcessResponseToPaymentConfirmation()
        {
            CreateMap<ProcessPaymentResponse, PaymentConfirmationDTO>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
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
