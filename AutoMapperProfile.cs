using AutoMapper;
using DataLayer.Model;
using DataLayer.Models.Event;
using DataLayer.Models.HallSector;
using DataLayer.Models.PlaceAddress;
using DataLayer.Models.PlaceHall;
using DataLayer.Models.Ticket;
using DataLayer.Models.TicketSeat;

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
        }
        private void MapEvent()
        {
            // CreateEvent -> Event
            CreateMap<CreateEvent, Event>();

            // UpdateEvent -> Event
            CreateMap<UpdateEvent, Event>()
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
            // CreateHallSector -> HallSector
            CreateMap<CreateHallSector, HallSector>();

            // UpdateHallSector -> HallSector
            CreateMap<UpdateHallSector, HallSector>()
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
            // CreatePlaceAddress -> PlaceAddress
            CreateMap<CreatePlaceAddress, PlaceAddress>();

            // UpdatePlaceAddress -> PlaceAddress
            CreateMap<UpdatePlaceAddress, PlaceAddress>()
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
            // CreatePlaceHall -> PlaceHall
            CreateMap<CreatePlaceHall, PlaceHall>();

            // UpdatePlaceHall -> PlaceHall
            CreateMap<UpdatePlaceHall, PlaceHall>()
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
            // CreateTicketSeat -> TicketSeat
            CreateMap<CreateTicketSeat, TicketSeat>();

            // UpdateTicketSeat -> TicketSeat
            CreateMap<UpdateTicketSeat, TicketSeat>()
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
            // CreateTicket -> Ticket
            CreateMap<CreateTicket, Ticket>();

            // UpdateTicket -> Ticket
            CreateMap<UpdateTicket, Ticket>()
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
