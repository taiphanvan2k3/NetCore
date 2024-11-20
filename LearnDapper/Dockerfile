# Sử dụng image chính thức của SQL Server
FROM mcr.microsoft.com/mssql/server

# Chấp nhận EULA và thiết lập mật khẩu cho tài khoản 'sa'
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=Taiphan@2403

# Cấu hình cổng 1433 để kết nối SQL Server
EXPOSE 1433

# Khởi động SQL Server khi container bắt đầu
CMD /opt/mssql/bin/sqlservr

# docker run -d --name sql_server_container -p 1433:1433 sql_server_image