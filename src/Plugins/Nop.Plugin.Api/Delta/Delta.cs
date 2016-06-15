﻿using System.Collections.Generic;
using Nop.Core.Infrastructure;
using Nop.Plugin.Api.Helpers;

namespace Nop.Plugin.Api.Delta
{
    public class Delta<TDto> where TDto : class, new()
    {
        private TDto _dto;

        // TODO: think of creating an instance of the mapping helper instead of using the same instance every time.
        // I think this will be nessecary because of the changed properties dictionary, so we can work with different 
        // dictionary every time an request is made.
        private IMappingHelper _mappingHelper = EngineContext.Current.Resolve<IMappingHelper>();

        public Dictionary<string, object> ChangedProperties { get; set; }

        public TDto Dto
        {
            get
            {
                if (_dto == null)
                {
                    _dto = new TDto();
                }

                return _dto;
            }
            set { _dto = value; }
        }
        
        public Delta(Dictionary<string, object> passedJsonPropertyValuePaires)
        {
            FillDto(passedJsonPropertyValuePaires);
            ChangedProperties = _mappingHelper.GetChangedProperties();
        }

        public void Merge<TEntity>(TEntity entity)
        {
            // here we work with object copy so we can perform the below optimization without affecting the actual object.
            Dictionary<string, object> changedProperties = ChangedProperties;

            var entityProperties = entity.GetType().GetProperties();

            // Set the changed properties
            foreach (var property in entityProperties)
            {
                // its a changed property so we need to update its value.
                if (changedProperties.ContainsKey(property.Name))
                {
                    // The value-type validation will happen in the model binder so here we expect the values to correspond to the types.
                    _mappingHelper.ConverAndSetValueIfValid(entity, property, changedProperties[property.Name]);
                  
                    // The remove operation is O(1) complexity. We are doing this for optimization purposes. 
                    // So we can break the loop if there are no more changed properties.
                    changedProperties.Remove(property.Name);
                }

                if (changedProperties.Count == 0) break;
            }
        }

        private void FillDto(Dictionary<string, object> passedJsonPropertyValuePaires)
        {
            _mappingHelper.SetValues(passedJsonPropertyValuePaires, Dto, typeof(TDto));
        }
    }
}