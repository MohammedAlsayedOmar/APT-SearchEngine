﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="APT.test" %>

<!DOCTYPE html>
<html>
	<head>
		<title>ASP.NET MVC - Pagination Example</title>
		<link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
	</head>
	<body>
		<div class="container">
			<div class="col-md-6 col-md-offset-3">
				<h1>ASP.NET MVC - Pagination Example</h1>

				<!-- items being paged -->
				<ul>
					@foreach (var item in Model.Items)
					{
						<li>@item</li>
					}
				</ul>				
				
				<!-- pager -->
				@if (Model.Pager.EndPage > 1)
				{
					<ul class="pagination">
						@if (Model.Pager.CurrentPage > 1)
						{
							<li>
								<a href="~/Home/Index">First</a>
							</li>
							<li>
								<a href="~/Home/Index?page=@(Model.Pager.CurrentPage - 1)">Previous</a>
							</li>
						}

						@for (var page = Model.Pager.StartPage; page <= Model.Pager.EndPage; page++)
						{
							<li class="@(page == Model.Pager.CurrentPage ? "active" : "")">
								<a href="~/Home/Index?page=@page">@page</a>
							</li>
						}

						@if (Model.Pager.CurrentPage < Model.Pager.TotalPages)
						{
							<li>
								<a href="~/Home/Index?page=@(Model.Pager.CurrentPage + 1)">Next</a>
							</li>
							<li>
								<a href="~/Home/Index?page=@(Model.Pager.TotalPages)">Last</a>
							</li>
						}
					</ul>
				}				
			</div>
		</div>
		<hr />
		<div class="credits text-center">
		  <p>
			  <a href="http://jasonwatmore.com/post/2015/10/30/ASPNET-MVC-Pagination-Example-with-Logic-like-Google.aspx">ASP.NET MVC - Pagination Example with Logic like Google</a>
		  </p>
		  <p>
			<a href="http://jasonwatmore.com">JasonWatmore.com</a>
		  </p>
		</div>
	</body>
</html>
