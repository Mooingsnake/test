set LUBAN_CLIENT=dotnet C:\Users\zy\Desktop\luban_examples-main\Tools\Luban.ClientServer\Luban.ClientServer.dll

%LUBAN_CLIENT% -j cfg ^
 --^
 --define_file  Defines\__root__.xml ^
 --input_data_dir Datas ^
 --output_data_dir ..\DataTables^
 --output_code_dir ..\Scripts\DataTables^
 --gen_types "code_cs_json,data_json" ^
 -s all
pause