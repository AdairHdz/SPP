﻿using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface ILinkedOrganizationRepository : IRepository<LinkedOrganization>
    {
        bool LinkedOrganizationIsAlreadyRegistered(LinkedOrganization linkedOrganization);
    }
}