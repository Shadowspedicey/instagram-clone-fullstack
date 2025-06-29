import { useEffect } from "react";
import { Route, Switch, useHistory } from "react-router";
import { NavLink } from "react-router-dom";
import { useMediaQuery } from "react-responsive";
import ChangePassword from "./ChangePassword";
import EditProfile from "./EditProfile";
import "./acc-edit.css";
import { useSelector } from "react-redux";

const AccountEdit = () =>
{
	const history = useHistory();
	const phoneQuery = useMediaQuery({query: "(max-width: 600px)"});
	const currentUser = useSelector(state => state.currentUser);

	useEffect(() =>
	{
		if (history.location.pathname === "/accounts/edit")
			document.title = "Edit Profile • Instadicey";
		else if (history.location.pathname === "/accounts/password/change")
			document.title = "Change Password • Instadicey";
	}, [history.location.pathname]);

	return(
		<div className="account-edit outlined">
			{ !phoneQuery &&
				<ul className="sidebar">
					<li><NavLink exact to="/accounts/edit" activeClassName="selected">Edit Profile</NavLink></li>
					{currentUser.username !== "guest" && <li><NavLink to="/accounts/password/change" activeClassName="selected">Change Password</NavLink></li>}
				</ul>
			}
			<div className="main">
				<Switch>
					<Route exact path="/accounts/edit" component={EditProfile}></Route>
					{currentUser.username !== "guest" && <Route exact path="/accounts/password/change" component={ChangePassword}></Route>}
				</Switch>
			</div>
		</div>
	);
};

export default AccountEdit;
