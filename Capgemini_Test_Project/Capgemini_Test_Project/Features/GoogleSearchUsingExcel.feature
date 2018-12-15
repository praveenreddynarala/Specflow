Feature: Perform google Search using the keyword Aviva with the help of Excel Data source
		 And Count Number of Links returned
		 And aslso print the linktext of 5th link displayed related to Aviva search


@ExcelReaderCase 
Scenario Outline: Perform google Search using the keyword Aviva with the help of Excel Data source
	Given user navigates to Goolgle home page
	When  user entered '<Search Text>' in the search text field
	Then user click on Google Search button
	Then verify the number of links returned on result page is : <No.of Links> 
	Then print the linktext of the '<Print Link No.>' th link displayed related to Aviva Search

@Source:TestData.xlsx
Examples: 
| Search Text | No.of Links | Print Link No. |
