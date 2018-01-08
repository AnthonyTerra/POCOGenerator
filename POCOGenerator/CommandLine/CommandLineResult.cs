using System;

namespace POCOGenerator.CommandLine
{
    public enum CommandLineResult
    {
        EmptyArgs = -1,
        NoErrors = 0,
        CommandLineParsingError = 1,
        CommandLineMutuallyExclusiveSetError = 2,
        MissingConnectionString = 3,
        MalformedConnectionString = 4,
        MissingExportFolder = 5,
        ExportFolderNotExist = 6,
        FileNameNotSet = 7,
        FileWritingError = 8,
        ExportError = 9,
        MissingIncludedObjects = 10
    }
}
