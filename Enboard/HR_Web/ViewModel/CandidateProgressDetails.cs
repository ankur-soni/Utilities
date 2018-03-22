using Data;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using AutoMapper;
using HR_Web.Utilities;
namespace HR_Web.ViewModel
{
    internal class CandidateProgressDetails
    {
        private IUserService _IUserService;
        private IRelationService _IRelationService;
        private ICandidateProgressDetailService _ICandidateProgressDetailService;
        private IEmploymentCountService _IEmploymentCountService;

        internal CandidateProgressDetails(IUserService IUserService, IRelationService IRelationService, ICandidateProgressDetailService ICandidateProgressDetailService, IEmploymentCountService IEmploymentCountService)
        {
            _IUserService = IUserService;
            _IRelationService = IRelationService;
            _ICandidateProgressDetailService = ICandidateProgressDetailService;
            _IEmploymentCountService = IEmploymentCountService;
        }

        internal CandidateGraphProgressDetailViewModel SaveCandidateProgressDetails(int userId)
        {
            var userDetails = _IUserService.GetById(userId);
            CandidateGraphProgressDetailViewModel candidateGraphProgressDetailViewModel = new CandidateGraphProgressDetailViewModel();
            try
            {
                CandidateGraphProgressDetail candidateGraphProgressDetail = new CandidateGraphProgressDetail();
                var canidateProgressBarList = _ICandidateProgressDetailService.GetAll(null, null, "");
                candidateGraphProgressDetail = canidateProgressBarList.FirstOrDefault(x => x.UserId == userId);

                if (canidateProgressBarList.Any() && candidateGraphProgressDetail != null)
                {
                    candidateGraphProgressDetail.UserId = userId;
                    candidateGraphProgressDetail.PersonalDetailsPercentage = GetPersonalDetailsPercentage(userDetails);
                    candidateGraphProgressDetail.EducationDetailsPercentage = GetEducationDetialsPercentage(userDetails);
                    candidateGraphProgressDetail.ContactDetailsPercentage = GetContactDetialsPercentage(userDetails);
                    candidateGraphProgressDetail.EmploymentDetailsPercentage = GetEmployementDetialsPercentage(userDetails);
                    candidateGraphProgressDetail.FamilyDetailsPercentage = GetFamilyDetialsPercentage(userDetails);
                    candidateGraphProgressDetail.UploadDcoumentsPercentage = GetUploadDocumentPercentage(userDetails);

                    bool status = _ICandidateProgressDetailService.Update(candidateGraphProgressDetail, null, "");
                }
                else
                {
                    candidateGraphProgressDetail = new CandidateGraphProgressDetail();
                    candidateGraphProgressDetail.UserId = userId;
                    candidateGraphProgressDetail.PersonalDetailsPercentage = GetPersonalDetailsPercentage(userDetails);
                    candidateGraphProgressDetail.EducationDetailsPercentage = GetEducationDetialsPercentage(userDetails);
                    candidateGraphProgressDetail.ContactDetailsPercentage = GetContactDetialsPercentage(userDetails);
                    candidateGraphProgressDetail.EmploymentDetailsPercentage = GetEmployementDetialsPercentage(userDetails);
                    candidateGraphProgressDetail.FamilyDetailsPercentage = GetFamilyDetialsPercentage(userDetails);
                    candidateGraphProgressDetail.UploadDcoumentsPercentage = GetUploadDocumentPercentage(userDetails);

                    _ICandidateProgressDetailService.Insert(candidateGraphProgressDetail, null, "");
                }

                Mapper.CreateMap<CandidateGraphProgressDetail, CandidateGraphProgressDetailViewModel>();
                candidateGraphProgressDetailViewModel = Mapper.Map<CandidateGraphProgressDetail, CandidateGraphProgressDetailViewModel>(candidateGraphProgressDetail);

                candidateGraphProgressDetailViewModel.AverragePercentage = (candidateGraphProgressDetailViewModel.PersonalDetailsPercentage
                                                                        + candidateGraphProgressDetailViewModel.ContactDetailsPercentage
                                                                        + candidateGraphProgressDetailViewModel.EducationDetailsPercentage
                                                                        + candidateGraphProgressDetailViewModel.EmploymentDetailsPercentage
                                                                        + candidateGraphProgressDetailViewModel.FamilyDetailsPercentage
                                                                        + candidateGraphProgressDetailViewModel.UploadDcoumentsPercentage) / Convert.ToDouble(6);
            }
            catch (Exception ex)
            {
                candidateGraphProgressDetailViewModel.ErrorMessage = ex.Message;
            }

            return candidateGraphProgressDetailViewModel;
        }

        private double GetPersonalDetailsPercentage(LoginDetail userDetails)
        {
            double personalDetailsPercentage = 0.0;
            if (userDetails != null)
            {
                var personalDetail = userDetails.EmployeePersonalDetails.FirstOrDefault(x => x.IsActive == true);
                if (personalDetail != null)
                {
                    String[] _chosenOtherDetails = new string[]
                                                {
                                                    Convert.ToString(personalDetail.FatherName),
                                                    Convert.ToString(personalDetail.Nationality.HasValue),
                                                    Convert.ToString(personalDetail.BloodGroup),
                                                    Convert.ToString(personalDetail.MaritalStatID),
                                                    Convert.ToString(personalDetail.SpouseName),
                                                    Convert.ToString(personalDetail.MotherTongue),
                                                    //Convert.ToString(personalDetail.AadharCardNumber), 
                                                    Convert.ToString(personalDetail.PANNumber),
                                                    //Convert.ToString(personalDetail.UANNumber),
                                                    Convert.ToString(personalDetail.PassportNumber),
                                                    Convert.ToString(personalDetail.NameOnPassport),
                                                    Convert.ToString(personalDetail.PassportExpiryDate.HasValue?personalDetail.PassportExpiryDate.Value:personalDetail.PassportExpiryDate)
                                                };
                    int acuatalCount = 0;
                    int totalFieldCount = _chosenOtherDetails.Length;
                    for (int i = 0; i < _chosenOtherDetails.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(_chosenOtherDetails[i]))
                        {
                            acuatalCount++;
                        }
                    }

                    if (!(personalDetail.HavePassport == "yes" ? true : false))
                    {
                        totalFieldCount = totalFieldCount - 3;
                    }

                    if (personalDetail.MaritalStatID == 2)
                    {
                        totalFieldCount = totalFieldCount - 1;
                    }

                    personalDetailsPercentage = ((Convert.ToDouble(acuatalCount) / Convert.ToDouble(totalFieldCount)) * Convert.ToDouble(100));
                }
            }
            return personalDetailsPercentage;
        }

        private double GetContactDetialsPercentage(LoginDetail userDetails)
        {
            double contactDetialsPercentage = 0.0;
            if (userDetails != null)
            {
                var contactDetail = userDetails.EmployeeContactDetails.FirstOrDefault();
                if (contactDetail != null)
                {
                    String[] _chosenOtherDetails = new string[]
                                                { 
                                                   // Convert.ToString(contactDetail.HomePhone), 
                                                    Convert.ToString(contactDetail.AnotherContact),
                                                    Convert.ToString(contactDetail.CurrentAddLine1),
                                                    Convert.ToString(contactDetail.CurrentCountryID),
                                                    Convert.ToString(contactDetail.CurrentStateID),
                                                    Convert.ToString(contactDetail.CurrentCityID),
                                                    //Convert.ToString(contactDetail.CurrentZipcode), 
                                                    Convert.ToString(contactDetail.PermanantAddLine1),
                                                    Convert.ToString(contactDetail.PermanantCountryID),
                                                    Convert.ToString(contactDetail.PermanantStateID),
                                                    Convert.ToString(contactDetail.PermanantCityID),
                                                  //  Convert.ToString(contactDetail.PermanantZipcode),
                                                };

                    int acuatalCount = 0;
                    int totalFieldCount = _chosenOtherDetails.Length;
                    for (int i = 0; i < _chosenOtherDetails.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(_chosenOtherDetails[i]))
                        {
                            acuatalCount++;
                        }
                    }

                    contactDetialsPercentage = ((Convert.ToDouble(acuatalCount) / Convert.ToDouble(totalFieldCount)) * Convert.ToDouble(100));
                }
            }
            return contactDetialsPercentage;
        }

        private double GetEmployementDetialsPercentage(LoginDetail userDetails)
        {
            double employementDetialsPercentage = 0.0;
            if (userDetails != null)
            {
                int averageCount = 0;
                var userEmployemntList = userDetails.EmploymentDetails.Where(x => x.IsActive == true);

                var employmentCount = 1;

                //if (employmentCount != null)
                //{
                var numberOfEmployments = employmentCount;
                if (userDetails.IsFresher ?? false)
                {
                    employementDetialsPercentage = Convert.ToDouble(100);
                }
                if (numberOfEmployments > 0)
                {
                    if (userEmployemntList.Any() && userEmployemntList.Count() != 0)
                    {
                        averageCount = userEmployemntList.Count();
                        if (averageCount >= numberOfEmployments)
                        {
                            employementDetialsPercentage = Convert.ToDouble(100);
                        }
                        else
                        {
                            employementDetialsPercentage = Convert.ToDouble(averageCount) / Convert.ToDouble(numberOfEmployments) * Convert.ToDouble(100);
                        }
                    }
                }
                //}
            }
            return employementDetialsPercentage;
        }

        private double GetEducationDetialsPercentage(LoginDetail userDetails)
        {
            double educationDetialsPercentage = 0.0;
            if (userDetails != null)
            {
                int averageCount = 0;
                var userEducationCategoryList = userDetails;

                if (userEducationCategoryList != null)
                {
                    var assignedEducationCategaries = userEducationCategoryList.AdminEducationCategoryForUsers.Where(x => x.IsActive == true).ToList();
                    if (assignedEducationCategaries.Any())
                    {
                        averageCount = assignedEducationCategaries.Count();
                        var userEducationList = userDetails.EmployeeEducationDetails.Where(x => x.IsActive == true);
                        if (userEducationList.Any())
                        {
                            var userEducationListJoin = (from item in userEducationList
                                                         join item2 in assignedEducationCategaries
                                                         on item.EducationCategoryID equals item2.EducationCategoryId
                                                         select item).ToList();

                            foreach (var item in userEducationListJoin)
                            {
                                String[] _chosenOtherDetails = new string[]
                                                        {
                                                            Convert.ToString(item.EducationCategoryID),
                                                            Convert.ToString(item.DisciplineID),
                                                            Convert.ToString(item.PassingYear),
                                                            Convert.ToString(item.ClassID),
                                                            Convert.ToString(item.OtherSpecialization),
                                                            Convert.ToString(item.CollegeID),
                                                            Convert.ToString(item.FromDate),
                                                            Convert.ToString(item.ToDate),
                                                            Convert.ToString(item.UniversityID),
                                                            Convert.ToString(item.Percentage),
                                                            Convert.ToString(item.BreaksDuringEducation),
                                                        };
                                int acuatalCount = 0;
                                int totalFieldCount = _chosenOtherDetails.Length;

                                for (int i = 0; i < _chosenOtherDetails.Length; i++)
                                {
                                    if (!String.IsNullOrEmpty(_chosenOtherDetails[i]))
                                    {
                                        acuatalCount++;
                                    }
                                }
                                educationDetialsPercentage = educationDetialsPercentage + ((Convert.ToDouble(acuatalCount) / Convert.ToDouble(totalFieldCount)) * Convert.ToDouble(100));
                            }
                            educationDetialsPercentage = educationDetialsPercentage / Convert.ToDouble(averageCount);
                        }

                    }

                }
            }
            return educationDetialsPercentage;
        }

        private double GetUploadDocumentPercentage(LoginDetail userDetails)
        {
            double uploadDocumentPercentage = 0.0;
            var contactDetail = userDetails.EmployeeContactDetails.FirstOrDefault();
            var requiredCountForAddress = 0;
            if (contactDetail != null)
            {
                requiredCountForAddress = contactDetail.IsBothAddSame == true ? 1 : 2;
            }
            if (userDetails != null && userDetails.DocumentDetails.Any())
            {
                List<DocumentDetail> userEducationDocumentCatList = new List<DocumentDetail>();
                var userEducationCatList = userDetails.AdminEducationCategoryForUsers.Where(x => x.IsActive == true).ToList();
                var documentDetails = userDetails.DocumentDetails.Where(x => x.IsActive == true);
                var educationalDocCount = 0;
                if ((userEducationCatList.Any() && userEducationCatList.Count != 0) && (documentDetails.Any() && documentDetails.Count() != 0))
                {
                    var a = documentDetails.Where(x => x.DocCatID == Constant.DocumentCategory.Education).ToList();
                    userEducationDocumentCatList = (from educationItem in userEducationCatList
                                                    join mappingItem in documentDetails.Where(x => x.DocCatID == Constant.DocumentCategory.Education).ToList()
                                                    on educationItem.Master_EducationCategory.EducationDocumentCategoryMappings.FirstOrDefault().DocumentID equals mappingItem.DocumentID
                                                    select mappingItem).ToList();
                    educationalDocCount = userEducationCatList.Count;
                }

                double employmentUploadPercentage = 0.0;
                if (userDetails.IsFresher ?? false)
                {
                    uploadDocumentPercentage = Convert.ToDouble(100);
                }
                if (documentDetails.Any() && userDetails.EmploymentDetails.Where(x => x.IsActive == true).Count() != 0)
                {
                    int totalCount = userDetails.EmploymentDetails.Where(x => x.IsActive == true).Count();
                    foreach (var item in userDetails.EmploymentDetails.Where(x => x.IsActive == true))
                    {

                        if (item.IsCurrentEmployment.HasValue && item.IsCurrentEmployment.Value)
                        {
                            employmentUploadPercentage = employmentUploadPercentage + GetDocumentPercentagEmployementCurrent(documentDetails.Where(x => x.DocCatID == Constant.DocumentCategory.Employment && x.EmploymentDetID == item.EmploymentDetID));
                        }
                        else
                        {
                            employmentUploadPercentage = employmentUploadPercentage + GetDocumentPercentagEmployementPast(documentDetails.Where(x => x.DocCatID == Constant.DocumentCategory.Employment && x.EmploymentDetID == item.EmploymentDetID));
                        }

                    }

                    uploadDocumentPercentage = (employmentUploadPercentage / Convert.ToDouble(totalCount));
                }
                uploadDocumentPercentage = uploadDocumentPercentage + GetDocumentPercentagEducation(userEducationDocumentCatList, educationalDocCount);
                uploadDocumentPercentage = uploadDocumentPercentage + GetDocumentPercentageAddressProof(documentDetails.Where(x => x.DocCatID == Constant.DocumentCategory.AddressProof), requiredCountForAddress);
                uploadDocumentPercentage = uploadDocumentPercentage + GetDocumentPercentageIDProof(documentDetails.Where(x => x.DocCatID == Constant.DocumentCategory.IdProof));

                uploadDocumentPercentage = (uploadDocumentPercentage / Convert.ToDouble(4));
            }
            return Math.Round(uploadDocumentPercentage, 2);
        }

        private double GetDocumentPercentageIDProof(IEnumerable<DocumentDetail> DocumentDetails)
        {
            double uploadDocumentPercentage = 0.0;
            int acuatalCount = 0;
            if (DocumentDetails.Any() && DocumentDetails.Count() != 0)
            {
                foreach (var item in DocumentDetails.GroupBy(x => x.DocumentID).Distinct())
                {
                    switch (item.FirstOrDefault() != null ? item.FirstOrDefault().DocumentID : 0)
                    {
                        case Constant.IDProof.PANCard:
                             acuatalCount++;
                            break;
                        //case Constant.IDProof.DrivingLicence: acuatalCount++;
                        //    break;
                        //case Constant.IDProof.VoterID: acuatalCount++;
                        //    break;
                        //case Constant.IDProof.MarriageCertificate: acuatalCount++;
                        //    break;
                        default:
                            break;
                    }
                }
                uploadDocumentPercentage = (Convert.ToDouble(acuatalCount) / Convert.ToDouble(1)) * Convert.ToDouble(100);
            }
            return uploadDocumentPercentage;
        }

        private double GetDocumentPercentageAddressProof(IEnumerable<DocumentDetail> DocumentDetails, int requiredCount)
        {
            double uploadDocumentPercentage = 0.0;
            int actualCount = 0;

            if (DocumentDetails.Any() && DocumentDetails.Count() != 0)
            {
                foreach (var item in DocumentDetails.GroupBy(x => x.DocumentID).Distinct())
                {
                    switch (item.FirstOrDefault() != null ? item.FirstOrDefault().DocumentID : 0)
                    {
                        case Constant.AddressProof.AadharCard:
                            actualCount++;
                            break;
                        case Constant.AddressProof.Passport:
                            actualCount++;
                            break;
                        case Constant.AddressProof.ElectricityBill:
                            actualCount++;
                            break;
                        case Constant.AddressProof.BSNLLandlineBill:
                            actualCount++;
                            break;
                        case Constant.AddressProof.NationalizedBanksAccountStatement:
                            actualCount++;
                            break;
                        case Constant.AddressProof.ValidRentAgreement:
                            actualCount++;
                            break;
                        case Constant.AddressProof.Rationcard:
                            actualCount++;
                            break;
                        case Constant.AddressProof.VoterId:
                            actualCount++;
                            break;
                        default:
                            break;
                    }
                }
                if (actualCount >= requiredCount)
                {
                    uploadDocumentPercentage = 100;
                }
                else
                {
                    uploadDocumentPercentage = (Convert.ToDouble(actualCount) / Convert.ToDouble(requiredCount)) * Convert.ToDouble(100);
                }
            }
            return uploadDocumentPercentage;
        }

        private double GetDocumentPercentagEmployementCurrent(IEnumerable<DocumentDetail> DocumentDetails)
        {
            double uploadDocumentPercentage = 0.0;
            int acuatalCount = 0;
            if (DocumentDetails.Any() && DocumentDetails.Count() != 0)
            {
                foreach (var item in DocumentDetails.GroupBy(x => x.DocumentID).Distinct())
                {
                    switch (item.FirstOrDefault() != null ? item.FirstOrDefault().DocumentID : 0)
                    {
                        case Constant.EmploymentProof.Latest3PaySlips:
                            acuatalCount++;
                            break;
                        case Constant.EmploymentProof.AppointmentLetter:
                            acuatalCount++;
                            break;
                        default:
                            break;
                    }
                }
                uploadDocumentPercentage = (Convert.ToDouble(acuatalCount) / Convert.ToDouble(2)) * Convert.ToDouble(100);
            }
            return uploadDocumentPercentage;
        }

        private double GetDocumentPercentagEmployementPast(IEnumerable<DocumentDetail> DocumentDetails)
        {
            double uploadDocumentPercentage = 0.0;
            int acuatalCount = 0;
            if (DocumentDetails.Any() && DocumentDetails.Count() != 0)
            {
                foreach (var item in DocumentDetails.GroupBy(x => x.DocumentID).Distinct())
                {
                    switch (item.FirstOrDefault() != null ? item.FirstOrDefault().DocumentID : 0)
                    {
                        case Constant.EmploymentProof.ExperienceLetter:
                            acuatalCount++;
                            break;
                        case Constant.EmploymentProof.Latest3PaySlips:
                            acuatalCount++;
                            break;
                        case Constant.EmploymentProof.AppointmentLetter:
                            acuatalCount++;
                            break;
                        default:
                            break;
                    }
                }
                uploadDocumentPercentage = (Convert.ToDouble(acuatalCount) / Convert.ToDouble(3)) * Convert.ToDouble(100);
            }
            return uploadDocumentPercentage;
        }

        private double GetDocumentPercentagEducation(IEnumerable<DocumentDetail> DocumentDetails, int requiredCount)
        {
            double uploadDocumentPercentage = 0.0;
            int actualCount = 0;
            //int totalCount = DocumentDetails.GroupBy(x => x.DocumentID).Distinct().Count();
            //if (totalCount != 0)
            //{
            //    foreach (var item in DocumentDetails.GroupBy(x => x.DocumentID).Distinct())
            //    {
            //        if (!String.IsNullOrWhiteSpace(item.FirstOrDefault() != null ? item.FirstOrDefault().DocumentName : null))
            //        {
            //            acuatalCount++;
            //        }
            //    }
            //    uploadDocumentPercentage = (Convert.ToDouble(acuatalCount) / Convert.ToDouble(totalCount)) * Convert.ToDouble(100);
            //}
            if (DocumentDetails.Any())
            {
                foreach (var item in DocumentDetails.GroupBy(x => x.DocumentID).Distinct())
                {
                    switch (item.FirstOrDefault() != null ? item.FirstOrDefault().DocumentID : 0)
                    {
                        case Constant.EducationProof.HSC:
                            actualCount++;
                            break;
                        case Constant.EducationProof.SSC:
                            actualCount++;
                            break;
                        case Constant.EducationProof.Graduation:
                            actualCount++;
                            break;
                        case Constant.EducationProof.Diploma:
                            actualCount++;
                            break;
                        case Constant.EducationProof.PostGraduateDiploma:
                            actualCount++;
                            break;
                        case Constant.EducationProof.PostGraduation:
                            actualCount++;
                            break;
                        default:
                            break;
                    }
                }
                uploadDocumentPercentage = (Convert.ToDouble(actualCount) / Convert.ToDouble(requiredCount)) * Convert.ToDouble(100);
            }
            return uploadDocumentPercentage;
        }

        private double GetFamilyDetialsPercentage(LoginDetail userDetails)
        {
            double educationDetialsPercentage = 0.0;
            int averageCount = 0;

            if (userDetails != null)
            {
                var relationList = _IRelationService.GetAll(null, null, "");


                var personalDetails = userDetails.EmployeePersonalDetails.FirstOrDefault(x => x.IsActive == true);
                var familydetails = userDetails.EmployeeFamilyDetails
                    .Where(x => x.IsActive == true && x.RelationshipID != Constants.Child).ToList();

                if (personalDetails != null && personalDetails.MaritalStatID == Constants.Single)
                {
                    relationList = relationList
                        .Where(r => r.RelationID != Constants.Spouse && r.RelationID != Constants.Child && r.RelationID != Constants.Relative).ToList();

                    if (familydetails.Any() && familydetails.Count != 0)
                    {
                        averageCount = relationList.Count();
                        foreach (var item in familydetails.Where(x => x.RelationshipID != Constants.Spouse))
                        {
                            if (!String.IsNullOrEmpty(item.FirstName) && !String.IsNullOrEmpty(item.LastName) && item.RelationshipID != Constants.Relative)
                                educationDetialsPercentage = educationDetialsPercentage + 100.00;
                            else
                                educationDetialsPercentage = educationDetialsPercentage + 0.00;
                        }
                        educationDetialsPercentage = educationDetialsPercentage / Convert.ToDouble(averageCount);
                    }
                }
                else
                {
                    var relList = relationList.Where(x => x.RelationID != Constants.Relative && x.RelationID != Constants.Child).Select(r => new { r.RelationID, r.RelationName })
                        .GroupBy(x => x.RelationID).ToList();

                    if (familydetails.Any() && familydetails.Count != 0)
                    {
                        averageCount = relList.Count();
                        foreach (var item in familydetails)
                        {
                            if (item.RelationshipID == Constants.Spouse)
                            {
                                int acuatalCount = 0;
                                String[] _chosenOtherDetails = new string[]
                                {
                                    Convert.ToString(item.FirstName),
                                    Convert.ToString(item.LastName),
                                    Convert.ToString(item.DOB),
                                    Convert.ToString(item.ContactNumber),
                                };

                                int totalFieldCount = _chosenOtherDetails.Length;

                                for (int i = 0; i < _chosenOtherDetails.Length; i++)
                                {
                                    if (!String.IsNullOrEmpty(_chosenOtherDetails[i]))
                                    {
                                        acuatalCount++;
                                    }
                                }
                                educationDetialsPercentage =
                                    educationDetialsPercentage +
                                    ((Convert.ToDouble(acuatalCount) / Convert.ToDouble(totalFieldCount)) *
                                     Convert.ToDouble(100));
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(item.FirstName) && !String.IsNullOrEmpty(item.LastName) &&
                                    item.RelationshipID != Constants.Relative &&
                                     item.RelationshipID != Constants.Child)
                                    educationDetialsPercentage = educationDetialsPercentage + 100.00;
                                else
                                    educationDetialsPercentage = educationDetialsPercentage + 0.00;
                            }
                        }

                        //var childDetails = userDetails.EmployeeFamilyDetails.Where(x => x.IsActive == true && x.RelationshipID == Constants.Child).FirstOrDefault();
                        //if (childDetails != null && !String.IsNullOrEmpty(childDetails.FirstName) && !String.IsNullOrEmpty(childDetails.LastName) && !String.IsNullOrEmpty(childDetails.Gender))
                        //{
                        //    educationDetialsPercentage = educationDetialsPercentage + 100.00;
                        //}
                        //else
                        //{
                        //    educationDetialsPercentage = educationDetialsPercentage + 0.00;
                        //}
                        educationDetialsPercentage = educationDetialsPercentage / Convert.ToDouble(averageCount);
                    }
                }
            }
            return educationDetialsPercentage;
        }
    }
}