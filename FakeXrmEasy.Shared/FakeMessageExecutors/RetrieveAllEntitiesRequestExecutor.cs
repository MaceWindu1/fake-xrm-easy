using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xrm.Sdk.Client;

namespace FakeXrmEasy.FakeMessageExecutors
{
    public class RetrieveAllEntitiesRequestExecutor : IFakeMessageExecutor
    {
        public bool CanExecute(OrganizationRequest request)
        {
            return request is RetrieveAllEntitiesRequest;
        }

        public OrganizationResponse Execute(OrganizationRequest request, XrmFakedContext ctx)
        {
            var req = request as RetrieveAllEntitiesRequest;

            if (req.EntityFilters.HasFlag(EntityFilters.Entity) || req.EntityFilters.HasFlag(EntityFilters.Attributes))
            {
                var entityMetadata = new EntityMetadataCollection();
                foreach (var item in ctx.Data.Keys)
                {
                    entityMetadata.Add(ctx.GetEntityMetadataByName(item));
                }
                var response = new RetrieveAllEntitiesResponse()
                {
                    Results = new ParameterCollection()
                    {
                        { "EntityMetadata", entityMetadata.ToArray() }
                    }
                };
                return response;
            }

            throw PullRequestException.PartiallyNotImplementedOrganizationRequest(req.GetType(), "logic for retrieving entity privileges and relationships metadata");
        }

        public Type GetResponsibleRequestType()
        {
            return typeof(RetrieveAllEntitiesRequest);
        }
    }
}
