import * as React from 'react';
import { connect } from 'react-redux';
import { Route } from 'react-router';

const Home = () => (
    <div id="container-home" className="container p-0 flex-magic">
        <div className="row m-0">
            <div className="col-12 p-0">
                <h1>About</h1>
                <p>
                    When GMing various RPGs, some of the hardest things to figure out are the names of your NPCs.
                    This was the source of the to start creating tools to make that part of GMing easier. The
                    start for now is in creating names for characters.  The next steps are to for building names
                    and then to help creating entire cities with a few clicks.
                </p>
                <p>
                    This idea finally came to fruition in my head when I ran across a series of articles called
                    "By Any Other Name" by <a href="//en.wikipedia.org/wiki/Owen_K.C._Stephens" target="_blank">Owen K.C. Stephens</a> in
                    old <a href="//en.wikipedia.org/wiki/Dragon_(magazine)" target="_blank">Dragon</a> magazines. It outlined
                    rules for randomly generating names for some of the major races of D&amp;D.  And then I clame
                    across the article for building names and the whole idea fell into place.
                </p>
                <p>
                    It's been years that I've tinkered with this, it's had a version or two that's popped up on the
                    internet and then disappeared again, but now I'm putting more time into it and trying to building
                    this out more.  Let's see if I can keep to a project for more than a few months at a time.
                </p>
            </div>
        </div>
    </div>
);

export default connect()(Home);
