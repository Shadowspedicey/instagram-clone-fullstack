import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { setSnackbar } from "../../state/actions/snackbar";
import { backend } from "../../config";
import { logOut } from "../../helpers";
import { useHistory } from "react-router-dom/cjs/react-router-dom.min";

const Like = ({size = 25, target, isComment}) =>
{
	const dispatch = useDispatch();
	const history = useHistory();
	const currentUser = useSelector(state => state.currentUser);
	const [isLiked, setIsLiked] = useState(false);
	useEffect(() =>
	{
		const checkIfLiked = async () =>
		{
			if (currentUser && target.likes.some(u => u.username === currentUser.username))
				setIsLiked(true);
		};
		checkIfLiked();
	}, [currentUser, target]);

	const like = async () =>
	{
		if (!currentUser) return alert("sign in");
		try
		{
			var result;
			if (isComment)
				result = await fetch(`${backend}/like/comment/${target.id}`, {
					method: "POST",
					headers: {
						Authorization: `Bearer ${localStorage.token}`
					}
				});
			else
				result = await fetch(`${backend}/like/post/${target.id}`, {
					method: "POST",
					headers: {
						Authorization: `Bearer ${localStorage.token}`
					}
				});

			if (result.status === 401)
				return logOut(dispatch, history);
			if (!result.ok) {
				const resultJSON = await result.json();
				throw new Error(resultJSON.detail, { cause: resultJSON.errors });
			}
			setIsLiked(true);
		} catch (err)
		{
			dispatch(setSnackbar(err.message ?? "Oops, try again later.", "error"));
		}
	};
	
	const unlike = async () =>
	{
		if (!currentUser) return alert("sign in");
		try
		{
			var result;
			if (isComment)
				result = await fetch(`${backend}/like/comment/remove/${target.id}`, {
					method: "POST",
					headers: {
						Authorization: `Bearer ${localStorage.token}`
					}
				});
			else
				result = await fetch(`${backend}/like/post/remove/${target.id}`, {
					method: "POST",
					headers: {
						Authorization: `Bearer ${localStorage.token}`
					}
				});

			if (result.status === 401)
				return logOut(dispatch, history);
			if (!result.ok) {
				const resultJSON = await result.json();
				throw new Error(resultJSON.detail, { cause: resultJSON.errors });
			}
			setIsLiked(false);
		} catch (err)
		{
			dispatch(setSnackbar(err.message ?? "Oops, try again later.", "error"));
		}
	};

	if (isLiked)
		return(
			<button className="like liked icon" onClick={unlike}>
				<svg xmlns="http://www.w3.org/2000/svg" width={size} height={size} viewBox="0 0 24 24"><path d="M12 4.248c-3.148-5.402-12-3.825-12 2.944 0 4.661 5.571 9.427 12 15.808 6.43-6.381 12-11.147 12-15.808 0-6.792-8.875-8.306-12-2.944z"/></svg>
			</button>
		);
	else return(
		<button className="like icon" onClick={like}>
			<svg xmlns="http://www.w3.org/2000/svg" width={size} height={size} viewBox="0 0 24 24" fillRule="evenodd" clipRule="evenodd"><path d="M12 21.593c-5.63-5.539-11-10.297-11-14.402 0-3.791 3.068-5.191 5.281-5.191 1.312 0 4.151.501 5.719 4.457 1.59-3.968 4.464-4.447 5.726-4.447 2.54 0 5.274 1.621 5.274 5.181 0 4.069-5.136 8.625-11 14.402m5.726-20.583c-2.203 0-4.446 1.042-5.726 3.238-1.285-2.206-3.522-3.248-5.719-3.248-3.183 0-6.281 2.187-6.281 6.191 0 4.661 5.571 9.429 12 15.809 6.43-6.38 12-11.148 12-15.809 0-4.011-3.095-6.181-6.274-6.181"/></svg>
		</button>
	);
};

export default Like;
