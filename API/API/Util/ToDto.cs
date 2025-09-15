using API.DTO;
using API.Model;

namespace API.Util
{
    public class ToDto
    {
        public static MemberDto MemberToMemberDto(Member member)
            => new()
            {
                Id = member.Id,
                DateOfBirth = member.DateOfBirth,
                ImageUrl = member.ImageUrl,
                UserName = member.UserName,
                Created = member.Created,
                LastActive = member.LastActive,
                Gender = member.Gender,
                Description = member.Description,
                City = member.City,
                Country = member.Country,
                Photos = member.Photos
            };

        public static IReadOnlyList<MemberDto> MembersToMemberDtos(IReadOnlyList<Member> members)
        {
            var membersList = new List<MemberDto>(members.Count);
            foreach (var member in members) membersList.Add(MemberToMemberDto(member));
            return membersList;
        }

        public static PhotoDto PhotoToPhotoDto(Photo photo)
            => new()
            {
                Id = photo.Id,
                Url = photo.Url,
                PublicId = photo.PublicId,
                MemberId = photo.MemberId,
            };

        public static IReadOnlyList<PhotoDto> PhotosToPhotoDtos(IReadOnlyList<Photo> photos)
        {
            var photoList = new List<PhotoDto>(photos.Count);
            foreach (var photo in photos) photoList.Add(PhotoToPhotoDto(photo));
            return photoList;
        }

        public static UserDto UserToUserDto(User user, string token)
            =>  new()
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Token = token,
                ImageUrl = user.ImageUrl,
            };

        public static void MemberUpdateDtoToMember(Member oldMember, MemberUpdateDto updatedMember)
        {
            oldMember.UserName = updatedMember.UserName ?? oldMember.UserName;
            oldMember.Description = updatedMember.Description ?? oldMember.Description;
            oldMember.City = updatedMember.City ?? oldMember.City;
            oldMember.Country = updatedMember.Country ?? oldMember.Country;
            oldMember.User.UserName = updatedMember.UserName ?? oldMember.UserName;
        }

        public static PaginatedResult<MemberDto> PRMemberToPRMemberDto(PaginatedResult<Member> members)
            => new()
            {
                MetaData = members.MetaData,
                Items = (List<MemberDto>)MembersToMemberDtos(members.Items)
            };

        public static MessageDto MessageToMessageDto(Message message)
            => new()
            {
                Id = message.Id,
                Content = message.Content,
                DateRead = message.DateRead,
                MessageSent = message.MessageSent,
                SenderId = message.SenderId,
                SenderUserName = message.Sender.UserName,
                SenderImageUrl = message.Sender.ImageUrl,
                RecipientId = message.RecipientId,
                RecipientUserName = message.Recipient.UserName,
                RecipientImageUrl = message.Recipient.ImageUrl,
            };

        public static IReadOnlyList<MessageDto> MessagesToMessageDtos(IReadOnlyList<Message> messages)
        {
            var messagesList = new List<MessageDto>(messages.Count);
            foreach (var message in messages) messagesList.Add(MessageToMessageDto(message));
            return messagesList;
        }

        public static PaginatedResult<MessageDto> PRMessageToPRMessageDto(PaginatedResult<Message> messages)
            => new()
            {
                MetaData = messages.MetaData,
                Items = (List<MessageDto>)MessagesToMessageDtos(messages.Items)
            };
    }
}
