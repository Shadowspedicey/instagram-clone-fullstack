import { useEffect, useRef, useState } from "react";
import { useDispatch } from "react-redux";
import { startLoading, stopLoading } from "../state/actions/isLoading";
import PostWindow from "./Posts/PostWindow";
import { backend } from "../config";
import { logOut } from "../helpers";
import { useHistory } from "react-router-dom/cjs/react-router-dom.min";

const HomePage = () =>
{
	const postsRef = useRef();
	const dispatch = useDispatch();
	const history = useHistory();
	// const [scroll, setScroll] = useState(0);
	const [olderPosts, setOlderPosts] = useState(null);
	const [postsToDisplay, setPostsToDisplay] = useState(null);

	// const handleScroll = () =>
	// {
	// 	const winScroll = document.body.scrollTop || document.documentElement.scrollTop;
	// 	const height = document.documentElement.scrollHeight - document.documentElement.clientHeight;
  	// setScroll(winScroll / height * 100);
	// };
	// useEffect(handleScroll, []);

	useEffect(() => document.title = "Instadicey", []);
	useEffect(() =>
	{
		const getPosts = async () =>
		{
			if (!localStorage.token) return;
			dispatch(startLoading());
			try {
				const result = await fetch(`${backend}/user/feed`, {
					headers: {
						Authorization: `Bearer ${localStorage.token}`
					}
				});
				if (result.status === 401)
					return logOut(dispatch, history);
				const resultJSON = await result.json();
				if (!result.ok)
					throw new Error(resultJSON.detail);

				const threeDaysAgo = new Date();
  				threeDaysAgo.setUTCDate(threeDaysAgo.getUTCDate() - 3);
				setPostsToDisplay(resultJSON.filter(p => new Date(p.createdAt+"Z") >= threeDaysAgo));
				setOlderPosts(resultJSON.filter(p => new Date(p.createdAt+"Z") < threeDaysAgo));
			} catch {

			}
			dispatch(stopLoading());
		};
		getPosts();
	}, [dispatch, history]);

	// useEffect(() =>
	// {
	// 	window.addEventListener("scroll", handleScroll);
	// 	return () =>
	// 	{
	// 		window.removeEventListener("scroll", handleScroll);
	// 	};
	// }, [postsRef]);

	if (!postsToDisplay) return null;
	return(
		<div className="home-page">
			{
				postsToDisplay &&
				<div className="posts" ref={postsRef}>
					{
						postsToDisplay.map(post =>
							<PostWindow post={post} isVertical key={post.id}/>)
					}
				</div>
			}
			{ olderPosts &&
					<div className="older-posts posts" ref={postsRef}>
						<h2>Showing posts older than 3 days</h2>
						{
							olderPosts.map(post =>
								<PostWindow post={post} isVertical key={post.id}/>)
						}
					</div>
			}
		</div>
	);
};

export default HomePage;
