import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as AncestryStore from '../../store/characterNameService';

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
    this.props.setSelectedAncestry(selectedValue);
    this.props.requestAncestryOptions();
  }

  public optionSelected(event: React.FormEvent<HTMLSelectElement>) {
    var selectedOption = event.currentTarget.value;
    this.props.setSelectedOption(selectedOption);
  }

  public setNameCount(event: React.FormEvent<HTMLSelectElement>) {
    var count = Number(event.currentTarget.value);
    this.props.setNameCount(count);
  }

  public requestCharacterNames(event: React.FormEvent<HTMLButtonElement>) {
    this.props.requestCharacterNames();
  }

  public render() {
    return (
      <React.Fragment>
        {this.renderAncestyDropdown()}
        {this.renderOptionsDropdown()}
        {this.renderCountDropdown()}
        {this.renderSubmitButton()}
        {this.renderName()}
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
          { this.props?.ancestries.sort((a, b) => a.sortOrder - b.sortOrder).map((ancestry: AncestryStore.Ancestry) =>
            <option key={ancestry.code} value={ancestry.code}>{ancestry.name}</option>
          )}
        </select>
      </div>
    );
  }

  private renderOptionsDropdown() {
    return (
      <div>
        <select onChange={ e => this.optionSelected(e) }>
          { this.props?.ancestryOptions?.options.sort((a, b) => a.sortOrder - b.sortOrder).map((option: AncestryStore.NameCodeSort) =>
            <option key={option.code} value={option.code}>{option.name}</option>
          )}
        </select>
      </div>
    );
  }

  private renderCountDropdown() {
    return (
      <div>
        <select onChange={ e=> this.setNameCount(e) }>
          <option selected={ this.props?.numberToGenerate === 1} value="1">1</option>
          <option selected={ this.props?.numberToGenerate === 2} value="2">2</option>
          <option selected={ this.props?.numberToGenerate === 3} value="3">3</option>
          <option selected={ this.props?.numberToGenerate === 4} value="4">4</option>
          <option selected={ this.props?.numberToGenerate === 5} value="5">5</option>
          <option selected={ this.props?.numberToGenerate === 6} value="6">6</option>
          <option selected={ this.props?.numberToGenerate === 7} value="7">7</option>
          <option selected={ this.props?.numberToGenerate === 8} value="8">8</option>
          <option selected={ this.props?.numberToGenerate === 9} value="9">9</option>
          <option selected={ this.props?.numberToGenerate === 10} value="10">10</option>
        </select>
      </div>
    )
  }

  private renderSubmitButton() {
    return (
      <div>
        <button type="button" onClick={e => this.requestCharacterNames(e) }>Generate Name</button>
      </div>
    );
  }

  private renderName() {
    return (
      <div>
        { this.props?.characterNames.map((name : AncestryStore.CharacterName) =>
          <div>
            <h3>{name.name}</h3>
            <h4>{name.ancestryName} | {name.optionName}</h4>
            <div>
              { name.definitions.map((definition : AncestryStore.CharacterNameDefinition) =>
                <div><b>{ definition.namePart}:</b> { definition.meanings.join(", ") }</div>
              )}
            </div>
          </div>
        )}
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.characterNames, // Selects which state properties are merged into the component's props
  AncestryStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
