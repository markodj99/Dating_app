using API.DTO;
using API.Interface;
using API.Model;
using System.ComponentModel.DataAnnotations;

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
    }
}
