using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace Infrastructure.Helpers;

public static class ApplicationHelpers
{

    public static string HandleMobileCounty(string zip)
    {
        //Hard coded for now, should be refactored into a database or configuration file

        string[] prichardZipCodes =
        {
            "36505",
            "36512",
            "36513",
            "36521",
            "36522",
            "36525",
            "36560",
            "36571",
            "36572",
            "36575",
            "36582",
            "36587",
            "36590",
            "36609",
            "36610",
            "36611",
            "36612",
            "36613",
            "36617",
            "36618",
            "36619",
            "36663",
            "36671",
            "36685",
            "36690",
            "36691",
            "36693"
        };

        return prichardZipCodes.Contains(zip) ? "49 02" : "49 02";
    }

    public static string HandleJeffersonCounty(string zip)
    {
        string[] bessemerZipCodes =
        {
            "35020",
            "35021",
            "35444",
            "35061",
            "35064",
            "35118",
            "35127",
            "35224",
            "35228",
            "35023",
            "35111",
            "35006",
            "35006",
            "35142"
        };
        return bessemerZipCodes.Contains(zip) ? "37 02" : "37 01";
    }

    public static string FormatAsPhoneNumber(string number, bool dashes = true)
    {
        if (string.IsNullOrWhiteSpace(number))
            return "";
        var pnum = Regex.Replace(number, "[^0-9.]", "");
        long num;
        return long.TryParse(pnum, out num) ? num.ToString(dashes ? "###-###-####" : "##########") : "";
    }

    public static XDocument checkAppXml(string applicationXml)
    {
        LogMsg(applicationXml, "app_xml");
        var doc = XDocument.Parse(applicationXml);
        var highestInstanceKey = 0;
        var blankElementList = new List<XElement>();
        var elementNames = new List<string>
        {
            "jobIncomeName",
            "selfEmploymentIncomeName",
            "roomAndBoardIncomeName",
            "unearnedIncomeName",
            "medicalExpenseExpenseIncurredBy",
            "dependentCareName",
            "childSupportWhoPays",
            "utilitiesHeatAcMember",
            "InvestmentsName",
            "BurialAccountName",
            "RetirementName",
            "SavingsName",
            "CheckingName",
            "cashOnHandName",
            "TrustFundsName",
            "OtherName"
        };

        //finds highest household member instance key
        foreach (var element in doc.Descendants("householdMember"))
        {
            int instanceKeyNumber;

            var instanceKey = element.Descendants("householdMemberInstanceKey").FirstOrDefault().Value;

            if (instanceKey != "")
                instanceKeyNumber = Convert.ToInt32(instanceKey.Replace("instance-", ""));
            else
                instanceKeyNumber = 0;

            if (instanceKeyNumber > highestInstanceKey)
                highestInstanceKey = instanceKeyNumber;
        }

        //gets unassigned income or expenses and assigns to head of household 
        foreach (var elementName in elementNames)
        foreach (var element in doc.Descendants(elementName))
            if (element.Value == "")
                element.ReplaceNodes(new XCData("instance-0"));
        //FLAG: income or expense auto-assigned to head of household

        //deletes non-numeric characters from room and board income meals per day field
        foreach (var element in doc.Descendants("roomAndBoardIncomeMealsPerDay"))
        {
            var elementValue = Regex.Replace(element.Value, "[^0-9.]", "");
            element.ReplaceNodes(new XCData(elementValue));
        }

        var members = doc.Descendants("householdMember");
        //fixes household member issues
        foreach (var element in members.ToList())
        {
            var isDuplicate = IsHouseholdMemberDuplicate(members, element);

            if (isDuplicate)
            {
                element.Remove();
            }
            else
            {
                var allBlank = true;
                var children = element.Descendants();
                var instanceKeyElement = element.Descendants("householdMemberInstanceKey").FirstOrDefault();
                //determines if household member entry is all blank
                foreach (var child in children)
                    if (!string.IsNullOrWhiteSpace(child.Value))
                        allBlank = false;
                if (allBlank)
                {
                    blankElementList.Add(element);
                }

                //adds instance key to household members with no instance key
                else if (instanceKeyElement != null && instanceKeyElement.Value == "")
                {
                    var newInstanceKey = "instance-" + (highestInstanceKey + 1);
                    instanceKeyElement.ReplaceNodes(new XCData(newInstanceKey));
                    //FLAG: household member given auto-generated instance key
                    highestInstanceKey++;
                }
            }
        }

        //removes blank household members
        foreach (var element in blankElementList)
            element.Remove();
        //FLAG: blank household member removed flag

        return doc;
    }
    
    public static void LogMsg(string msg, string fileName)
    {
        var sb = new StringBuilder();
        sb.Append(msg);
        System.IO.File.WriteAllText($"C:/temp/" + $"{fileName}.{DateTime.Now.Ticks.ToString()}.txt", sb.ToString());
        sb.Clear();
    }

    public static bool IsHouseholdMemberDuplicate(IEnumerable<XElement> members, XElement member)
    {
        var fName = member.Element("householdMemberFirstName").Value;
        var lName = member.Element("householdMemberLastName").Value;
        var dob = member.Element("householdMemberDOB").Value;
        //var ssn = member.Element("householdMemberSocialSecurityNumber").Value;

        var num = 0;

        foreach (var _member in members)
        {
            var _fName = _member.Element("householdMemberFirstName").Value;
            var _lName = _member.Element("householdMemberLastName").Value;
            var _dob = _member.Element("householdMemberDOB").Value;
            //var _ssn = _member.Element("householdMemberSocialSecurityNumber").Value;
            if (
                fName == _fName &&
                lName == _lName &&
                dob == _dob
                // && (ssn == _ssn)
            )
                num = num + 1;
        }

        if (num > 1) return true;
        return false;
    }
}