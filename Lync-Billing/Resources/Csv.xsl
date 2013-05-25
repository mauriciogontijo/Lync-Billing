xslstylesheet version=1.0 xmlnsxsl=httpwww.w3.org1999XSLTransform
	xsloutput method=text 

	xsltemplate match=records
		xslapply-templates select=record 
	xsltemplate

	xsltemplate match=record
		xslfor-each select=
			xsltextxsltext	
			xslvalue-of select=. 
			xsltextxsltext
			
			xslif test=position() != last()
				xslvalue-of select=',' 
			xslif
		xslfor-each
		xsltext&#10;xsltext
	xsltemplate

xslstylesheet