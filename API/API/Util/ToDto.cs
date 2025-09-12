using API.DTO;
using API.Model;

namespace API.Util
{
    public class ToDto
    {
        public static MemberDto MemberToMemberDto(Member member)
        {
            return new MemberDto
            {
                Id = member.Id,
                DateOfBirth = member.DateOfBirth,
                ImageUrl = member.ImageUrl,
                Username = member.Username,
                Created = member.Created,
                LastActive = member.LastActive,
                Gender = member.Gender,
                Description = member.Description,
                City = member.City,
                Country = member.Country,
                Photos = member.Photos
            };
        }

        public static IReadOnlyList<MemberDto> MembersToMemberDtos(IReadOnlyList<Member> members)
        {
            var membersList = new List<MemberDto>(members.Count);

            foreach (var member in members)
            {
                membersList.Add(MemberToMemberDto(member)); 
            }

            return membersList;
        }

        public static PhotoDto PhotoToPhotoDto(Photo photo)
        {
            return new PhotoDto
            {
                Id = photo.Id,
                Url = photo.Url,
                PublicId = photo.PublicId,
                MemberId = photo.MemberId,
            };
        }

        public static IReadOnlyList<PhotoDto> PhotosToPhotoDtos(IReadOnlyList<Photo> photos)
        {
            var photoList = new List<PhotoDto>(photos.Count);

            foreach (var photo in photos)
            {
                photoList.Add(PhotoToPhotoDto(photo));
            }

            return photoList;
        }

        public static UserDto UserToUserDto(User user, string token)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                ImageUrl = user.ImageUrl,
            };
        }

        public static void MemberUpdateDtoToMember(Member oldMember, MemberUpdateDto updatedMember)
        {
            oldMember.Username = updatedMember.Username ?? oldMember.Username;
            oldMember.Description = updatedMember.Description ?? oldMember.Description;
            oldMember.City = updatedMember.City ?? oldMember.City;
            oldMember.Country = updatedMember.Country ?? oldMember.Country;

            oldMember.User.Username = updatedMember.Username ?? oldMember.Username;
        }

        public static PaginatedResult<MemberDto> PRMemberToPRMemberDto(PaginatedResult<Member> members)
        {
            return new PaginatedResult<MemberDto>
            {
                MetaData = members.MetaData,
                Items = (List<MemberDto>)MembersToMemberDtos(members.Items)
            };
        }

        public static MessageDto MessageToMessageDto(Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                Content = message.Content,
                DateRead = message.DateRead,
                MessageSent = message.MessageSent,
                SenderId = message.SenderId,
                SenderUsername = message.Sender.Username,
                SenderImageUrl = message.Sender.ImageUrl,
                RecipientId = message.RecipientId,
                RecipientUsername = message.Recipient.Username,
                RecipientImageUrl = message.Recipient.ImageUrl,
            };
        }
    }
}
