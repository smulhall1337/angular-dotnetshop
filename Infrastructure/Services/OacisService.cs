using Core.Entities.Dhr;
using Core.Interfaces;
using Infrastructure.Data.Oacis;

namespace Infrastructure.Services
{
    public class OacisService: IOacisService
    {
        public string GetLinkedCase(Guid userId)
        {
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {

                var link = ctx.CASE.FirstOrDefault(l => l.MYDHR_USERID.HasValue && l.MYDHR_USERID.Value == userId);
                if (link == null)
                    return "NOT LINKED";
                else if (link.NO_CASE.ToString().Length == 8)
                {
                    // NO_CASE is numeric in OACIS, all case numbers must be 9 digits
                    // If NO_CASE is 8 digits long, we need to add a leading zero
                    return "0" + link.NO_CASE.ToString();
                }
                else
                {
                    // If NO_CASE is 9 digits long. return 
                    return link.NO_CASE.ToString();
                }
            }
        }

        public string LinkCase(OacisLinkCaseDto dto)
        {
            int pinInt = 0;
            if (!int.TryParse(dto.Pin, out pinInt))
                return "6";

            decimal caseNumberDec = 0;
            if (!decimal.TryParse(dto.CaseNumber, out caseNumberDec))
                return "5";

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var link =
                    ctx.CASE.FirstOrDefault(l => l.NO_CASE == caseNumberDec);

                if (link == null)
                    return "4";

                if (link.MYDHR_USERID.HasValue && !link.MYDHR_USERID.Value.Equals(dto.MyAlabamaUserId))
                    return "3";

                if (link.MYDHR_PIN.Trim() != dto.Pin.Trim())
                    return "2";

                //Check to see if SSN exists on case
                bool ssnExists = false;
                dto.Ssn = dto.Ssn.Replace("-", string.Empty).Trim();
                if (dto.Ssn != "")
                {
                    var hoh = link.MEMBER.FirstOrDefault(h => h.FL_HOH == "Y");
                    if (!hoh.NO_SSN.ToLower().Equals(dto.Ssn.ToLower()))
                    {
                        foreach (var fam in link.MEMBER)
                        {
                            if (fam.NO_SSN.ToLower().Equals(dto.Ssn.ToLower()))
                                ssnExists = true;
                        }
                    }
                    else
                    {
                        ssnExists = true;
                    }
                }

                if (ssnExists)
                {
                    link.MYDHR_USERID = dto.MyAlabamaUserId;
                    link.MYDHR_LINKDATE = DateTime.Now;

                    ctx.SaveChanges();
                    return "0";
                }
                return "1";
            }
        }

        public string UnlinkCase(OacisLinkCaseDto dto)
        {
            int caseNumberInt = 0;
            if (!int.TryParse(dto.CaseNumber, out caseNumberInt))
                return "BAD PIN";

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var link =
                    ctx.CASE.FirstOrDefault(
                        l => (l.NO_CASE == caseNumberInt));

                if (link == null)
                    return "BAD PIN";

                link.MYDHR_USERID = null;
                link.MYDHR_LINKDATE = null;

                ctx.SaveChanges();
            }

            return "UNLINKED";
        }

        public string RepresentativeCanViewCase(RepresentativeCanViewCaseDto dto)
        {
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                decimal dCaseNumber = 0M;
                if (!decimal.TryParse(dto.CaseNumber, out dCaseNumber))
                    return "CASE NOT FOUND";

                var caseCheck = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);
                if (caseCheck == null)
                    return "CASE NOT FOUND";

                var hoh = caseCheck.MEMBER.FirstOrDefault(h => h.FL_HOH == "Y");

                //TODO: Might need more logic depending on how ARs are stored in this table
                var ar = caseCheck.AR_RPRSNTTV.FirstOrDefault(r => r.FL_EBT_RPRSNTTV == "N");

                //Check to see if SSN exists on case
                bool ssnExists = false;
                if (dto.Ssn.Trim() != "")
                {

                    if (!hoh.NO_SSN.Equals(dto.Ssn.Replace("-", string.Empty).Trim()))
                    {
                        foreach (var fam in caseCheck.MEMBER)
                        {
                            if (fam.NO_SSN.Equals(dto.Ssn.Trim()))
                                ssnExists = true;
                        }
                    }
                }

                //Check to see if Name exists on case
                bool nameExists = false;
                if (!ssnExists)
                {
                    string hohFirstName = hoh.NM_MMBR_FRST.ToUpper().Trim();
                    string hohLastName = hoh.NM_MMBR_LST.ToUpper().Trim();
                    string arFirstName = ar.NM_RPRSNTTV_FRST.ToUpper().Trim();
                    string arLastName = ar.NM_RPRSNTTV_LAST.ToUpper().Trim();

                    if (!
                        ((hohFirstName.Equals(dto.FirstName.ToUpper().Trim()) &&
                          hohLastName.Equals(dto.LastName.ToUpper().Trim())) ||
                         (arFirstName.Equals(dto.FirstName.ToUpper().Trim()) &&
                          arLastName.Equals(dto.LastName.ToUpper().Trim()))
                        ))
                    {
                        foreach (var fam in caseCheck.MEMBER)
                        {
                            if (fam.NM_MMBR_FRST.ToUpper().Trim().Equals(dto.FirstName.ToUpper().Trim()) &&
                                fam.NM_MMBR_LST.ToUpper().Trim().Equals(dto.LastName.ToUpper().Trim()))
                                nameExists = true;
                        }
                    }
                    else
                    {
                        nameExists = true;
                    }
                }

                if (ssnExists || nameExists)
                {
                    return "ON CASE";
                }

                return "NOT ON CASE";
            }
        }

        public string GetCaseFoodAssistanceWorkerEmail(string caseNumber)
        {
            string email = "";
                //ConfigurationManager.AppSettings["GeneralEmail"];
            decimal dCaseNumber = 0M;

            if (!decimal.TryParse(caseNumber, out dCaseNumber))
                return email;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var dhrCase = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (dhrCase == null)
                    return email;

                var countyEmail = ctx.REF_COUNTY_GROUP_EMAIL.FirstOrDefault(e => e.ID_CNTY_STATE == dhrCase.REF_COUNTY_STATE.ID_CNTY_STATE &&
                    e.CD_TRCKNG_OFFC == dhrCase.CD_TRCKNG_OFFC);

                if (countyEmail == null)
                    return email;

                email = countyEmail.TX_EMAIL;
            }
            return email;
        }

        public MailingAddress GetCountyOffice(string caseNumber)
        {
            //DHR State Street Address
            MailingAddress address = new MailingAddress();
            address.Address1 = "50 North Ripley Street";
            address.City = "Montgomery";
            address.State = "AL";
            address.ZipCode = "36130";
            address.Valid = true;

            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return address;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var dhrCase = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (dhrCase == null)
                    return address;

                var dhrAddress = ctx.REF_CNTY_ADDRESS.OrderBy(a => a.ID_CNTY_STATE).FirstOrDefault(a => a.ID_CNTY_STATE == dhrCase.REF_COUNTY_STATE.ID_CNTY_STATE);

                if (dhrAddress == null)
                    return address;

                address.Address1 = dhrAddress.AD_STREET1;
                address.Address1 = dhrAddress.AD_STREET2;
                address.City = dhrAddress.AD_CITY;
                address.State = "AL";
                address.ZipCode = dhrAddress.AD_ZIP;
                address.Valid = true;
            }

            return address;
        }

        public string GetCountyFromCountyCode(string countyCode)
        {
            decimal dCountyCode = 0;
            if (!Decimal.TryParse(countyCode, out dCountyCode))
                return "";

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var county = ctx.REF_COUNTY_STATE.FirstOrDefault(c => c.CD_CNTY == dCountyCode);
                if (county != null)
                    return county.CD_CNTY_DSCR.ToString();
                else
                    return "";
            }
        }

        public string GetCountyOfficePhone(string caseNumber)
        {
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                int dCaseNumber;

                if (!int.TryParse(caseNumber, out dCaseNumber))
                    return "";

                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);
                if (cas == null)
                    return "";

                var office = ctx.REF_CNTY_ADDRESS.FirstOrDefault(c => c.ID_CNTY_STATE == cas.ID_CNTY_STATE
                    && c.OFFICE_CODE == cas.CD_TRCKNG_OFFC);
                if (office == null)
                    return "";

                Int64 num;
                if (!Int64.TryParse(office.NO_OFFICE_PHONE, out num))
                    return "";

                return num.ToString("###-###-####");

            }
        }

        public MailingAddress GetCaseAddresses(GetCaseAddressesDto dto)
        {
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                MailingAddress address = new MailingAddress();
                decimal dCaseNumber;

                if (!decimal.TryParse(dto.CaseNumber, out dCaseNumber))
                    return address;

                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return address;

                var hoh = cas.MEMBER.FirstOrDefault(m => m.FL_HOH == "Y");

                if (hoh == null)
                    return address;

                int rAddressType = 3;
                if (dto.Type.ToUpper() == "M")
                    rAddressType = 4;

                var mAddress = hoh.ADDRESS.FirstOrDefault(a => a.ID_REF_ADDRSS_TYPE == rAddressType);

                if (mAddress == null)
                    return address;

                address.Address1 = mAddress.AD_STREET1 != null ? mAddress.AD_STREET1.Trim() : "";
                address.Address2 = mAddress.AD_STREET2 != null ? mAddress.AD_STREET2.Trim() : "";
                address.City = mAddress.AD_CITY != null ? mAddress.AD_CITY.Trim() : "";
                address.State = mAddress.AD_STATE != null ? mAddress.AD_STATE.Trim() : "";
                address.ZipCode = mAddress.AD_ZIP != null ? mAddress.AD_ZIP.Trim() : "";

                return address;
            }
        }

        public string GetCaseStatus(string caseNumber)
        {
            string ret = "CASE NOT FOUND";
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                decimal dCaseNumber;

                if (!decimal.TryParse(caseNumber, out dCaseNumber))
                    return ret;

                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return ret;

                ret = cas.CASE_STATUS.REF_CASE_STATUS_TYPE.TX_CASE_STTS_DSCR;

                return ret;
            }
        }

        public DateTime GetNextRecertificationDate(string caseNumber)
        {
            DateTime ret = DateTime.MinValue;
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                decimal dCaseNumber;

                if (!decimal.TryParse(caseNumber, out dCaseNumber))
                    return ret;

                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return ret;

                if (cas.CASE_STATUS.DT_CERT_END.HasValue)
                {
                    ret = cas.CASE_STATUS.DT_CERT_END.GetValueOrDefault();
                    //Date always needs to be start of month
                    ret = new DateTime(ret.Year, ret.Month, 1);
                }

                return ret;
            }
        }

        public DateTime GetCertificationStartDate(string caseNumber)
        {
            DateTime ret = DateTime.MinValue;
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                decimal dCaseNumber;

                if (!decimal.TryParse(caseNumber, out dCaseNumber))
                    return ret;

                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return ret;

                if (cas.CASE_STATUS.DT_CERT_STRT.HasValue)
                {
                    ret = cas.CASE_STATUS.DT_CERT_STRT.GetValueOrDefault();
                }
                return ret;
            }
        }

        public DateTime GetCertificationEndDate(string caseNumber)
        {
            DateTime ret = DateTime.MinValue;
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                decimal dCaseNumber;

                if (!decimal.TryParse(caseNumber, out dCaseNumber))
                    return ret;

                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return ret;

                if (cas.CASE_STATUS.DT_CERT_END.HasValue)
                {
                    ret = cas.CASE_STATUS.DT_CERT_END.GetValueOrDefault();
                }
                return ret;
            }
        }

        public string GetHeadOfHouseholdName(string caseNumber)
        {
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                string ret = "UNKNOWN";
                decimal dCaseNumber;

                if (!decimal.TryParse(caseNumber, out dCaseNumber))
                    return ret;
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return ret;

                var hoh = cas.MEMBER.FirstOrDefault(h => h.FL_HOH.NullTrim() == "Y");

                if (hoh == null)
                    return ret;

                ret = string.Format("{0} {1}", hoh.NM_MMBR_FRST.NullTrim(), hoh.NM_MMBR_LST.NullTrim());

                return ret;
            }
        }

        public string GetCaseCounty(string caseNumber)
        {
            string ret = "UNKNOWN";

            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return ret;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var dhrCase = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (dhrCase == null)
                    return ret;

                ret = dhrCase.REF_COUNTY_STATE.CD_CNTY_DSCR.Trim();
            }
            return ret;
        }

        public VerificationItemDto[] GetVerificationItems(string caseNumber)
        {
            List<VerificationItemDto> items = new List<VerificationItemDto>();

            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return items.ToArray();

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var dhrCase = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (dhrCase == null)
                    return items.ToArray();

                int caseIdNumber = dhrCase.ID_CASE_NMBR;

                var caseVerifications = ctx.Procedures.Get_VRFCTNS_CASEAsync((int?)caseIdNumber).Result;
                var otherVerifications = ctx.Procedures.Get_ALL_VRFCTNS_CRYSTLRPTAsync((int?)caseIdNumber, "I").Result;

                foreach (var verification in caseVerifications)
                {
                    VerificationItemDto item = new VerificationItemDto();
                    item.Type = "case";
                    item.Member = verification.MEMBER_NAME.Trim();

                    if (verification.ID_VRFCTN_ATHRZD_RPRSNTTV.GetValueOrDefault() > 0)
                        item.Text = "Verification is needed for the Authorized Representative. " + verification.TXT_DTL_CASE.NullTrim();
                    else if (verification.VRFHOH.GetValueOrDefault() > 0)
                        item.Text = "Verification is needed for identity of the head of household. " + verification.TXT_DTL_CASE.NullTrim();
                    else if (verification.VRFHHCOMP.GetValueOrDefault() > 0)
                        item.Text = "Verification is needed for household composistion. " + verification.TXT_DTL_CASE.NullTrim();
                    else if (verification.VRFHHRSDNCY.GetValueOrDefault() > 0)
                        item.Text = "Verification is needed for residency. " + verification.TXT_DTL_CASE.NullTrim();
                    else if (verification.VRFINTRVWEE.GetValueOrDefault() > 0)
                        item.Text = "Verification is needed for the identity of the interviewee. " + verification.TXT_DTL_CASE.NullTrim();

                    items.Add(item);
                }

                foreach (var verification in otherVerifications)
                {
                    VerificationItemDto item = new VerificationItemDto();
                    item.Member = verification.MEMBER_NAME.Trim();

                    List<string> mandatoryVerificationsTypes = new List<string>()
                    {"ALIEN","FA", "EMPLMNT","UI_SLFEMP","UI_STRKR","STRIKER","RSRC","UNERND","UI_CS_RCVD",
                        "UI_DEP_CARE_OP","UI_SHLTR_OP","UI_INS_OP","UI_MED_OP"};

                    if (mandatoryVerificationsTypes.Contains(verification.VRFCTN_TYPE))
                        item.Type = "mandatory";
                    else
                        item.Type = "optional";
                    item.Text = "Verification is needed for " + verification.VRFCN_INFO.NullTrim() + ". " + verification.TXT_DETAIL.NullTrim();
                    items.Add(item);
                }
            }
            return items.ToArray();
        }


        public AuthRepDto[] GetAuthorizedReps(string caseNumber)
        {
            List<AuthRepDto> items = new List<AuthRepDto>();

            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return items.ToArray();

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var dhrCase = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (dhrCase == null)
                    return items.ToArray();

                int caseIdNumber = dhrCase.ID_CASE_NMBR;

                var authReps = ctx.AR_RPRSNTTV.Where(c => c.ID_CASE_NMBR == caseIdNumber);

                foreach (var item in authReps)
                {
                    AuthRepDto rep = new AuthRepDto();
                    rep.FirstName = item.NM_RPRSNTTV_FRST.NullTrim();
                    rep.MiddleName = item.NM_RPRSNTTV_MDL.NullTrim();
                    rep.LastName = item.NM_RPRSNTTV_LAST.NullTrim();
                    rep.Suffix = item.NM_RPRSNTTV_SFX.NullTrim();
                    if (item.FL_EBT_RPRSNTTV == "Y")
                        rep.EbtRep = true;
                    else
                        rep.EbtRep = false;
                    items.Add(rep);
                }
            }
            return items.ToArray();
        }

        public AllotmentDto[] GetAllotmentHistory(string caseNumber)
        {
            List<AllotmentDto> items = new List<AllotmentDto>();

            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return items.ToArray();

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var dhrCase = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (dhrCase == null)
                    return items.ToArray();

                //while (caseNumber.Length < 9)
                //    caseNumber = "0" + caseNumber;

                var allotments = ctx.SCI2_ISSUNCE.Where(c => c.ISS_NO_CASE == caseNumber);

                foreach (var item in allotments)
                {
                    AllotmentDto allo = new AllotmentDto();
                    allo.IssueAmount = item.ISS_AMT.NullTrim();
                    allo.ClaimPayment = item.ISS_RECOUP_AMT.NullTrim();
                    allo.PriorMonthBenifit = item.ISS_RESTOR_AMT.NullTrim();
                    allo.AvailableDate = item.ISS_BEN_AVAIL_DT.NullTrim() == "0 0 0 0" ? DateTime.MinValue : DateTime.ParseExact(item.ISS_BEN_AVAIL_DT.NullTrim(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    allo.TransactionDate = item.ISS_TRAN_DT.NullTrim() == "0 0 0 0" ? DateTime.MinValue : DateTime.ParseExact(item.ISS_TRAN_DT.NullTrim(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    allo.InitialAmount = item.ISS_INITIAL_AMT.NullTrim();
                    allo.IssuanceType = item.ISS_TYPE.NullTrim();
                    items.Add(allo);
                }
            }
            return items.ToArray();
        }

        public decimal GetIncomeLimit(string caseNumber)
        {
            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return 0;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                int numMembers = cas.MEMBER.Count;

                var incomeLimitTable = ctx.Procedures.Get_REF_INCOME_LIMITAsync().Result;

                var entry = incomeLimitTable.ToList().FirstOrDefault(a => Convert.ToInt32(a.NO_HH_SIZE) == numMembers);

                return entry.AM_INCM_LIMIT.GetValueOrDefault();
            }
        }

        public decimal GetTotalChildSupportPayments(string caseNumber)
        {
            decimal sum = 0;

            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return sum;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                foreach (var fam in cas.MEMBER)
                {
                    //var payments = ctx.GetByIdMmbr_SumChildSupportIncome(fam.ID_MMBR, "S").ToList();
                    List<Core.Entities.Oacis.Expense_Proration_CRResult>? paymentData = ctx.Procedures.Expense_Proration_CRAsync(cas.ID_CASE_NMBR, "C", "C", "N").Result.ToList();
                    if (paymentData.Count > 0)
                        sum += paymentData.First().PRRTD_2;
                    //sum = paymentData.PAID_AMNT;
                    //foreach (GetByIdMmbr_SumChildSupportIncome_Result payment in payments)
                    //{
                    //    sum += payment.TOTAL_UNEARNED_INC_AMNT.GetValueOrDefault();
                    //}
                }
                return sum;
            }
        }

        public IncomeExpenseEntryDto[] GetIncomesAndChildSupport(string caseNumber)
        {
            List<IncomeExpenseEntryDto> items = new List<IncomeExpenseEntryDto>();

            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return items.ToArray();

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return items.ToArray();

                int caseId = cas.ID_CASE_NMBR;

                List<Core.Entities.Oacis.GetByIDCaseNmbr_UnearnedIncomeTotalResult>? unearnedIncomes = ctx.Procedures.GetByIDCaseNmbr_UnearnedIncomeTotalAsync(caseId, "C", "E").Result.ToList();

                List<Core.Entities.Oacis.GetByIDCaseNmbr_EarnedIncomeTotalsResult>? earnedIncomes = ctx.Procedures.GetByIDCaseNmbr_EarnedIncomeTotalsAsync(caseId, "C").Result.ToList();

                List<Core.Entities.Oacis.GetIndividualCSAmountsResult>? childSupport = ctx.Procedures.GetIndividualCSAmountsAsync(caseId, "C", "E").Result.ToList();

                foreach (var fam in cas.MEMBER)
                {
                    IncomeExpenseEntryDto entry = new IncomeExpenseEntryDto();
                    entry.FirstName = fam.NM_MMBR_FRST;
                    entry.LastName = fam.NM_MMBR_LST;
                    entry.TotalChildSupport = 0;
                    entry.TotalEarned = 0;
                    entry.TotalUnearned = 0;

                    entry.UnearnedIncomes = unearnedIncomes
                        .Where(a => a.ID_MMBR == fam.ID_MMBR)
                        .Select(b => new MemberIncomeExpense()
                        {
                            Amount = b.TOTAL_UNEARNED_INCOME2.GetValueOrDefault(0.0M),
                            Type = b.UNEARNED_TYPE
                        }).ToArray();

                    entry.TotalUnearned = entry.UnearnedIncomes
                        .Sum(b => b.Amount);


                    entry.EarnedIncomes = earnedIncomes
                        .Where(a => a.ID_MMBR == fam.ID_MMBR)
                        .Select(b => new MemberIncomeExpense()
                        {
                            Amount = b.TOTAL_EARNED_INCOME2.GetValueOrDefault(0.0M),
                            Type = b.NM_EMPLYR
                        }).ToArray();

                    entry.TotalEarned = entry.EarnedIncomes
                        .Sum(b => b.Amount);


                    entry.ChildSupportDeductions = childSupport
                        .Where(a => a.MMBR_PAYING == fam.ID_MMBR)
                        .Select(b => new MemberIncomeExpense()
                        {
                            Amount = b.CS_PAID_MNTHLY_AMNT.GetValueOrDefault(0.0M),
                            Type = ""
                        }).ToArray();

                    entry.TotalChildSupport = entry.ChildSupportDeductions
                        .Sum(b => b.Amount);

                    if (entry.UnearnedIncomes.Count() != 0 || entry.EarnedIncomes.Count() != 0 || entry.ChildSupportDeductions.Count() != 0)
                        items.Add(entry);
                }

                return items.ToArray();
            }
        }
        
        public DateTime GetNextAppointment(string caseNumber)
        {
            DateTime ret = DateTime.MinValue;
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                decimal dCaseNumber;

                if (!decimal.TryParse(caseNumber, out dCaseNumber))
                    return ret;

                var registration = ctx.REGISTER_APPLICATION.FirstOrDefault(c => c.NO_CASE == caseNumber);

                if (registration != null && registration.DT_CASE_APPTDATETIME.HasValue)
                    ret = registration.DT_CASE_APPTDATETIME.GetValueOrDefault();
                else
                {
                    var appointmentForm = ctx.FORM_APPOINTMENT.FirstOrDefault(c => c.NO_CASE == caseNumber);
                    if (appointmentForm != null && appointmentForm.DT_OFF_IVIEW.HasValue)
                        ret = appointmentForm.DT_OFF_IVIEW.GetValueOrDefault();
                    else if (appointmentForm != null)
                        ret = appointmentForm.DT_TELE_IVIEW.GetValueOrDefault();
                }

                return ret;
            }
        }

        public string GetOfficePhone(string caseNumber)
        {
            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return "";

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var dhrCase = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (dhrCase == null)
                    return "";

                var dhrAddress = ctx.REF_CNTY_ADDRESS.OrderBy(a => a.ID_CNTY_STATE).FirstOrDefault(a => a.ID_CNTY_STATE == dhrCase.REF_COUNTY_STATE.ID_CNTY_STATE);

                if (dhrAddress == null)
                    return "";

                return dhrAddress.NO_OFFICE_PHONE;
            }
        }

        public DateTime GetSemiAnnualReviewDate(string caseNumber)
        {
            DateTime ret = DateTime.MinValue;
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                decimal dCaseNumber;

                if (!decimal.TryParse(caseNumber, out dCaseNumber))
                    return ret;

                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return ret;

                if (cas.CASE_STATUS.DT_CERT_STRT.HasValue)
                    ret = cas.CASE_STATUS.DT_CERT_STRT.GetValueOrDefault().AddMonths(5);

                return ret;
            }
        }

        public DateTime GetStatusDate(string caseNumber)
        {
            decimal dCaseNumber = 0;

            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return DateTime.MinValue;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return DateTime.MinValue;

                return cas.CASE_STATUS.DT_CASE_STTS ?? DateTime.MinValue;
            }
        }

        public DateTime GetAnnualReviewDate(string caseNumber)
        {
            DateTime ret = DateTime.MinValue;
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                decimal dCaseNumber;

                if (!decimal.TryParse(caseNumber, out dCaseNumber))
                    return ret;

                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return ret;

                if (cas.CASE_STATUS.DT_CERT_STRT.HasValue)
                {
                    ret = cas.CASE_STATUS.DT_CERT_STRT.GetValueOrDefault().AddMonths(11);
                    DateTime endDate = ret.AddMonths(1);
                    endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month), 23, 59, 59);
                    if (DateTime.Today > endDate)
                        ret = cas.CASE_STATUS.DT_CERT_STRT.GetValueOrDefault().AddMonths(23);
                }

                return ret;
            }
        }

        public string[] GetHouseholdNames(string caseNumber)
        {
            List<string> items = new List<string>();

            decimal dCaseNumber = 0;
            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return items.ToArray();

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return items.ToArray();

                foreach (var fam in cas.MEMBER)
                {
                    if (fam.CD_CASE_CRTD.NullTrim() == "S")
                    {
                        string name = fam.NM_MMBR_FRST.NullTrim() + " " + fam.NM_MMBR_LST.NullTrim();
                        items.Add(name);
                    }
                }

                return items.ToArray();
            }
        }

        public string GetReviewCode(string caseNumber)
        {
            decimal dCaseNumber = 0;

            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return null;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null || cas.REPORTING == null)
                    return null;

                var rep = cas.REPORTING.FirstOrDefault();
                if (rep == null)
                    return null;

                return rep.CD_RPRTNG_RQRMNT.NullTrim();
            }
        }

        public string GetClosureCode(string caseNumber)
        {
            decimal dCaseNumber = 0;

            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return null;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);

                if (cas == null)
                    return null;

                return cas.CASE_STATUS.CD_RJCTN_CLSR.NullTrim();
            }
        }

        public bool UserCanAccessVerifications(string caseNumber)
        {
            decimal dCaseNumber = 0;

            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return false;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);
                if (cas == null)
                    return false;

                DateTime denialDate = cas.CASE_STATUS.DT_CASE_STTS ?? DateTime.MinValue;
                var closureCode = cas.CASE_STATUS.CD_RJCTN_CLSR.NullTrim();
                var statusCode = cas.CASE_STATUS.ID_REF_CASE_STATUS_TYPE;

                if (DateTime.Now <= denialDate.AddDays(30))
                {
                    if (statusCode == 5 && closureCode == "14")
                        return true;

                    if (statusCode == 7 && closureCode == "14")
                        return true;

                    if (statusCode == 17 && closureCode == "14")
                        return true;
                }
            }
            return false;
        }

        public AllotmentDto GetSNAPVerification(string caseNumber)
        {
            decimal dCaseNumber = 0;

            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return null;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);
                if (cas == null)
                    return null;

                IEnumerable<AllotmentDto> allotments = GetAllotmentHistory(caseNumber);
                AllotmentDto allotment = allotments.OrderByDescending(a => a.AvailableDate).FirstOrDefault();
                if (allotment == null)
                {
                    return null;
                }

                bool benefitsThisMonth;

                if (allotment.TransactionDate.Month == DateTime.Now.Month && allotment.TransactionDate.Year == DateTime.Now.Year)
                    benefitsThisMonth = true;
                else
                    benefitsThisMonth = false;

                if (benefitsThisMonth && allotment.IssuanceType == "1")
                {
                    return allotment;
                }
            }
            return null;
        }

        public bool UserCanAccessComplaint(string caseNumber)
        {
            decimal dCaseNumber = 0;

            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return false;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);
                if (cas == null)
                    return false;

                DateTime denialDate = cas.CASE_STATUS.DT_CASE_STTS ?? DateTime.MinValue;

                if (DateTime.Now <= denialDate.AddDays(90))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UserIsOnDNAList(string SSN)
        {
            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                if (!String.IsNullOrWhiteSpace(SSN))
                {
                    var ssn = SSN.Replace("-", "");
                    var dna = ctx.REGISTER_DONOTAPPLY.FirstOrDefault(u => u.ID_SSN == ssn);
                    if (dna == null)
                        return false;
                    else
                    {
                        if ((dna.RECERTFLAG != null && dna.RECERTFLAG.ToLower() == "y") || !string.IsNullOrWhiteSpace(dna.CASENBRAPP) || dna.CERTEND == null)
                            return false;

                        else if (dna.CERTEND != null)
                        {
                            string certMonth = dna.CERTEND.Substring(4);
                            string certYear = dna.CERTEND.Substring(0, 4);
                            string certDay = DateTime.DaysInMonth(Convert.ToInt32(certYear), Convert.ToInt32(certMonth)).ToString();
                            DateTime certDate = DateTime.Parse(certMonth + "/" + certDay + "/" + certYear);

                            if ((certDate - DateTime.Today).Days > 45)
                                return true;
                        }
                    }
                }
                return false;
            }
        }

        public Guid GetUserIdByCaseNumber(string caseNumber)
        {
            if (caseNumber.Length != 9)
            {
                return Guid.Empty;
            }
            decimal dCaseNumber = 0;

            if (!Decimal.TryParse(caseNumber, out dCaseNumber))
                return Guid.Empty;

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var cas = ctx.CASE.FirstOrDefault(c => c.NO_CASE == dCaseNumber);
                if (cas == null)
                    return Guid.Empty;

                return cas.MYDHR_USERID ?? Guid.Empty;
            }
        }

        public async Task<string> CheckHoliday(string checkDate)
        {
            string isHoliday = "n";

            DateTime cDate = new DateTime(int.Parse(checkDate.Substring(6, 4)), int.Parse(checkDate.Substring(0, 2)), int.Parse(checkDate.Substring(3, 2)), 0, 0, 0);

            using (MyDhrOacisContext ctx = new MyDhrOacisContext())
            {
                var result = await ctx.Procedures.IMPORT_CheckHolidayAsync(cDate);

                if (result == 0) isHoliday = "n";

                else if (result == 1) isHoliday = "y";
            }

            return isHoliday;
        }
     

        public string RepresentativeCanViewCase(string caseNumber, string ssn, string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        MailingAddressDto IOacisService.GetCountyOffice(string caseNumber)
        {
            throw new NotImplementedException();
        }

        public MailingAddressDto GetCaseAddresses(string type, string caseNumber)
        {
            throw new NotImplementedException();
        }

        public RegisterApplicationDto GetCompletedApplicationByCaseNumber(string caseNumber)
        {
            throw new NotImplementedException();
        }
    }
}   
