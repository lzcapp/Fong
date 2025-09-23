using System.Text.Json;
using Fong.Models;
using Fong.Data.Models;

namespace Fong.Extensions {
    public static class ModelMappingExtensions {
        // Device mappings
        public static DeviceEntity ToEntity(this Device device) {
            return new DeviceEntity {
                Mac = device.Mac,
                Ip = JsonSerializer.Serialize(device.Ip),
                State = device.State,
                Name = device.Name,
                Type = device.Type,
                Make = device.Make,
                Model = device.Model,
                ContactId = device.ContactId,
                FirstSeen = device.FirstSeen,
                LastChanged = device.LastChanged
            };
        }
        
        public static Device ToModel(this DeviceEntity entity) {
            List<string> ipList = new();
            if (!string.IsNullOrEmpty(entity.Ip)) {
                try {
                    ipList = JsonSerializer.Deserialize<List<string>>(entity.Ip) ?? new List<string>();
                } catch {
                    ipList = new List<string>();
                }
            }
            
            return new Device {
                Mac = entity.Mac,
                Ip = ipList,
                State = entity.State,
                Name = entity.Name,
                Type = entity.Type,
                Make = entity.Make,
                Model = entity.Model,
                ContactId = entity.ContactId,
                FirstSeen = entity.FirstSeen,
                LastChanged = entity.LastChanged
            };
        }
        
        // Contact mappings
        public static ContactEntity ToEntity(this Contact contact) {
            return new ContactEntity {
                ContactId = contact.Info?.ContactId ?? string.Empty,
                DisplayName = contact.Info?.DisplayName ?? string.Empty,
                ContactType = contact.Info?.ContactType ?? string.Empty,
                PictureImageData = contact.Info?.PictureImageData,
                PictureUrl = contact.Info?.PictureUrl
            };
        }
        
        public static Contact ToModel(this ContactEntity entity) {
            return new Contact {
                Info = new ContactInfo {
                    ContactId = entity.ContactId,
                    DisplayName = entity.DisplayName,
                    ContactType = entity.ContactType,
                    PictureImageData = entity.PictureImageData,
                    PictureUrl = entity.PictureUrl
                }
            };
        }
        
        // AgentInfo mappings
        public static AgentInfoEntity ToEntity(this AgentInfo agentInfo) {
            return new AgentInfoEntity {
                Ip = agentInfo.Ip,
                ModelName = agentInfo.ModelName,
                State = agentInfo.State,
                AgentId = agentInfo.AgentId,
                FriendlyName = agentInfo.FriendlyName,
                DeviceType = agentInfo.DeviceType,
                Manufacturer = agentInfo.Manufacturer
            };
        }
        
        public static AgentInfo ToModel(this AgentInfoEntity entity) {
            return new AgentInfo(
                entity.Ip,
                entity.ModelName,
                entity.State,
                entity.AgentId,
                entity.FriendlyName,
                entity.DeviceType,
                entity.Manufacturer
            );
        }
    }
}