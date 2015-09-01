# OceanPDFSearch
Ocean PDF Search is a desktop application to search PDF files on the file system.

## 1. Searching

### 1.1 Common Information
You can search for PDFs that contains all words or execute a proximity search, where two words must be present within a certain range. The search works case-insensitive which means that "UML" and "uml" will yield the same documents. An exact search with quotes is currently not supported. Please use instead the proximity search!

### 1.2 Search for all words
The search for "software test UML" finds all PDFs, where the words "software", "test" and "UML" are present. You can search for as many words as you want.

### 1.3 Proximity search
The search "software <3> test" finds all PDFs, where the word "software" is surrounded by the word "test" within a range of three words. You can use any desired range. The proximity search will work only with two words. If you want to search for more terms, please use a nested search.

### 1.4 Nested searches
You can combine several searches to search further within the previous results. Example: "software <3> test | UML". Now, the system searches for PDFs with "software" surrounded by "test" within a range of three words and the resulting PDFs are searches for the word "UML". You can nest any count of searches!

## 2. Setup
- Please download the latest [release](https://github.com/SommerEngineering/OceanPDFSearch/releases) and extract the archive into your PDF collection's base directory
- You can organize your PDF documents within subfolder
- Start the OceanPDFSearch.exe file
- Click on "Select PDF Viewer" and select your PDF application. Ocean PDF Search was tested successful with Tracker Software's [PDF-XChange Viewer](http://www.tracker-software.com/product/pdf-xchange-viewer) and the [PDF-XChange Editor](http://www.tracker-software.com/product/pdf-xchange-editor).
- Click on "Index" to build your PDF index. This can take several hours for huge collections!
- While the indexing process is running, you can query the already known documents
