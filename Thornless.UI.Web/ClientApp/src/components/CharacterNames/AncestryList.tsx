import * as React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../../store';
import * as AncestryStore from '../../store/CharacterNames';

// At runtime, Redux will merge together...
type AncestryProps =
  AncestryStore.CharacterNameState
  & typeof AncestryStore.actionCreators


class FetchData extends React.Component<AncestryProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  public ancestrySelected(event: React.FormEvent<HTMLSelectElement>) {
    var selectedValue = event.currentTarget.value;
    this.props.requestAncestryOption(selectedValue);
  }

  public render() {
    return (
      <React.Fragment>
        {this.renderAncestyDropdown()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestAncestries();
  }

  private renderAncestyDropdown() {
    return (
      <div>
        <select onChange={ e => this.ancestrySelected(e) }>
          { this.props?.ancestries.map((ancestry: AncestryStore.Ancestry) =>
            <option key={ancestry.code} value={ancestry.code}>{ancestry.name}</option>
          )}
        </select>
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.characterNames, // Selects which state properties are merged into the component's props
  AncestryStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
